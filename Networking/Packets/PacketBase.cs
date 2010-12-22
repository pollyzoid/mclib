using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MCLib.Enums;

namespace MCLib.Networking.Packets
{
    public abstract class PacketBase
    {
        #region Fields

        private Packet? _id;

        public Packet Id
        {
            get { return (Packet)(_id ?? (_id = GetType().GetCustomAttributes(false).OfType<PacketAttribute>().Single().Id)); }
        }

        private static readonly Dictionary<Packet, Func<PacketBase>> ClientPackets;
        private static readonly Dictionary<Packet, Func<PacketBase>> ServerPackets;

        #endregion

        #region Methods

        public virtual void Read(NetworkStreamMC stream)
        {
            foreach (var p in GetType().GetProperties().Where(p => typeof (PacketBase).GetProperty(p.Name) == null))
            {
                p.SetValue(this, stream.GetType().InvokeMember(p.PropertyType.Name, BindingFlags.InvokeMethod, null, stream, null), null);
            }
        }

        protected virtual void Write(PacketWriter writer)
        {
            foreach (var p in GetType().GetProperties().Where(p => typeof(PacketBase).GetProperty(p.Name) == null))
            {
                writer.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, writer, new[] { p.GetValue(this, null) });
            }
        }

        public void Send(NetworkStreamMC stream)
        {
            var writer = new PacketWriter {(byte) Id};
            Write(writer);
            stream.Write(writer.ToArray(), 0, writer.Count());
        }

	    #endregion

        #region Static methods

        static PacketBase()
        {
            ClientPackets = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof (PacketBase))
                            &&
                            (t.GetCustomAttributes(false).OfType<PacketAttribute>().Single().Side == PacketSide.Shared
                             ||
                             t.GetCustomAttributes(false).OfType<PacketAttribute>().Single().Side == PacketSide.Client))
                .ToDictionary(
                    t => t.GetCustomAttributes(false).OfType<PacketAttribute>().Single().Id,
                    t => (Func<PacketBase>) (() => (PacketBase) Activator.CreateInstance(t)));

            ServerPackets = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof (PacketBase))
                            &&
                            (t.GetCustomAttributes(false).OfType<PacketAttribute>().Single().Side == PacketSide.Shared
                             ||
                             t.GetCustomAttributes(false).OfType<PacketAttribute>().Single().Side == PacketSide.Server))
                .ToDictionary(
                    t => t.GetCustomAttributes(false).OfType<PacketAttribute>().Single().Id,
                    t => (Func<PacketBase>) (() => (PacketBase) Activator.CreateInstance(t)));
        }

        public static PacketBase TagFromId(Packet id, HandlerMode mode)
        {
            if (mode == HandlerMode.Client)
            {
                if (!ClientPackets.ContainsKey(id))
                {
                    throw new NotImplementedException(string.Format("Invalid client packet 0x{0:X2}", (byte)id));
                }

                return ClientPackets[id]();
            }

            if (!ServerPackets.ContainsKey(id))
            {
                throw new NotImplementedException(string.Format("Invalid server packet 0x{0:X2}", (byte)id));
            }

            return ServerPackets[id]();
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PacketAttribute : Attribute
    {
        public Packet Id;
        public PacketSide Side = PacketSide.Shared;
    }
}
