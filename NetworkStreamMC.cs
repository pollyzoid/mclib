using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MCLib
{
    public class NetworkStreamMC
    {
        #region Fields

        private readonly NetworkStream _stream;

        #endregion

        #region Constructor

        public NetworkStreamMC(NetworkStream stream)
        {
            _stream = stream;
        }

        #endregion

        #region Reading

        public byte ReadByte()
        {
            return (byte)_stream.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            var buff = new byte[count];
            int recv = 0;
            while (recv < count)
                recv += _stream.Read(buff, recv, count - recv);

            return buff;
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(SwapEndian(ReadBytes(2)), 0);
        }

        public int ReadInt32()
        {
            return BitConverter.ToInt32(SwapEndian(ReadBytes(4)), 0);
        }

        public long ReadInt64()
        {
            return BitConverter.ToInt64(SwapEndian(ReadBytes(8)), 0);
        }

        public float ReadSingle()
        {
            return BitConverter.ToSingle(SwapEndian(ReadBytes(4)), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(SwapEndian(ReadBytes(8)), 0);
        }

        public string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes(ReadInt16()));
        }

        #endregion

        #region Writing

        public void Write(byte[] buffer, int offset, int size)
        {
            _stream.Write(buffer, offset, size);
        }

        #endregion

        #region Helpers

        private static byte[] SwapEndian(IEnumerable<byte> bytes)
        {
            return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes.ToArray();
        }

        #endregion
    }
}
