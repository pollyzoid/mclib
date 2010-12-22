using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCLib.Networking
{
    public class PacketWriter : IEnumerable<byte>
    {
        #region Fields

        private readonly List<byte> _list = new List<byte>();

        #endregion

        #region Public methods
        public void Add(byte arg)
        {
            _list.Add(arg);
        }

        public void Add(bool arg)
        {
            _list.Add(arg ? (byte)1 : (byte)0);
        }

        public void Add(short arg)
        {
            _list.AddRange(SwapEndian(BitConverter.GetBytes(arg)));
        }

        public void Add(int arg)
        {
            _list.AddRange(SwapEndian(BitConverter.GetBytes(arg)));
        }

        public void Add(long arg)
        {
            _list.AddRange(SwapEndian(BitConverter.GetBytes(arg)));
        }

        public void Add(float arg)
        {
            _list.AddRange(SwapEndian(BitConverter.GetBytes(arg)));
        }

        public void Add(double arg)
        {
            _list.AddRange(SwapEndian(BitConverter.GetBytes(arg)));
        }

        public void Add(string arg)
        {
            Add((short)arg.Length);
            _list.AddRange(Encoding.UTF8.GetBytes(arg));
        }

        public void Add(byte[] arg)
        {
            _list.AddRange(arg);
        }

        public void Add(short[] args)
        {
            foreach(var a in args)
            {
                Add(a);
            }
        }
        #endregion

        #region Helpers

        private static IEnumerable<byte> SwapEndian(IEnumerable<byte> bytes)
        {
            return BitConverter.IsLittleEndian ? bytes.Reverse() : bytes;
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<byte> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
