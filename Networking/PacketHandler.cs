﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using MCLib.Enums;
using MCLib.Networking.Packets;

namespace MCLib.Networking
{
    /// <summary>
    /// Determines which packet protocol to use
    /// </summary>
    public enum HandlerMode
    {
        /// <summary>
        /// Server-to-client protocol
        /// </summary>
        Server,
        /// <summary>
        /// Client-to-server protocol
        /// </summary>
        Client
    }

    /// <summary>
    /// Handles networking to the server
    /// </summary>
    public class PacketHandler
    {
        #region Fields

        private bool _eventMode;

        public bool EventMode
        {
            get { return _eventMode; }
            set
            {
                lock (_locker)
                {
                    _eventMode = value;

                    foreach (var packet in _recvPackets)
                        FirePacket(packet);
                }
            }
        }

        private bool _isActive = true;
        public bool IsActive
        {
            get { return _isActive; }
            private set
            {
                lock (_locker)
                {
                    _isActive = value;
                }
            }
        }

        public HandlerMode Mode { get; private set; }

        // Packets
        public delegate void PacketCallback(PacketBase packet);

        public event PacketCallback UnsubscribedPacket;

        private readonly Dictionary<Packet, PacketCallback> _subscriptions =
            new Dictionary<Packet, PacketCallback>();

        private readonly Queue<PacketBase> _recvPackets = new Queue<PacketBase>();

        // Networking

        private readonly NetworkStreamMC _stream;

        private readonly TcpClient _tcp = new TcpClient();

        // Threading

        private readonly AutoResetEvent _waitForPacket = new AutoResetEvent(false);

        private readonly object _locker = new object();

        private readonly Thread _packetHandler;

        #endregion

        #region Constructor

        public PacketHandler(string host, int port, HandlerMode mode)
        {
            Mode = mode;

            _tcp = new TcpClient(host, port);
            
            _stream = new NetworkStreamMC(_tcp.GetStream());

            _packetHandler = new Thread(Worker)
                                 {
                                     Name = "SC#PacketHandler",
                                     IsBackground = true
                                 };

            _packetHandler.Start();
        }

        #endregion

        #region Public methods

        public void Subscribe(Packet id, PacketCallback func)
        {
            if (!_subscriptions.ContainsKey(id))
            {
                _subscriptions[id] = null;
            }

            _subscriptions[id] += func;
        }

        public PacketBase ReceivePacket()
        {
            if (EventMode)
                throw new InvalidOperationException("Cannot receive packet in event mode");

            lock (_locker)
            {
                if (_recvPackets.Count > 0)
                {
                    return _recvPackets.Dequeue();
                }
            }

            _waitForPacket.WaitOne();

            lock (_locker)
            {
                return _recvPackets.Dequeue();
            }
        }

        public void SendPacket(PacketBase packet)
        {
            if (!IsActive) return;

            packet.Send(_stream);
        }

        public void Disconnect()
        {
            IsActive = false;
        }

        #endregion

        #region Private methods

        void FirePacket(PacketBase packet)
        {
            if (_subscriptions.ContainsKey(packet.Id))
            {
                _subscriptions[packet.Id](packet);
            }
            else if (UnsubscribedPacket != null)
            {
                UnsubscribedPacket(packet);
            }
            else
            {
                throw new NotSupportedException("Unhandled packet");
            }
        }

        void Worker()
        {
            int numPackets = 0;

            while (IsActive)
            {
                byte id = _stream.Byte();
                numPackets++;

                // If we're on client mode, we need to get the server-version of the packet,
                // because that's the packet sent by the server
                var packet = PacketBase.TagFromId((Packet)id, Mode == HandlerMode.Client ? HandlerMode.Server : HandlerMode.Client);

                packet.Read(_stream);

                if (EventMode)
                {
                    FirePacket(packet);
                }
                else
                {
                    lock(_locker)
                    {
                        _recvPackets.Enqueue(packet);
                        _waitForPacket.Set();
                    }
                }

                if (numPackets > 50)
                {
                    SendPacket(new KeepAlive());
                }
            }

            _packetHandler.Join();
        }

        #endregion
    }
}
