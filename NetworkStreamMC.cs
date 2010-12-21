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

        public byte[] Bytes(int count)
        {
            var buff = new byte[count];
            int recv = 0;
            while (recv < count)
                recv += _stream.Read(buff, recv, count - recv);

            return buff;
        }
        
        public byte Byte()
        {
            return (byte)_stream.ReadByte();
        }

        public bool Boolean()
        {
            return Byte() != 0;
        }

        public short Int16()
        {
            return BitConverter.ToInt16(SwapEndian(Bytes(2)), 0);
        }

        public int Int32()
        {
            return BitConverter.ToInt32(SwapEndian(Bytes(4)), 0);
        }

        public long Int64()
        {
            return BitConverter.ToInt64(SwapEndian(Bytes(8)), 0);
        }

        public float Single()
        {
            return BitConverter.ToSingle(SwapEndian(Bytes(4)), 0);
        }

        public double Double()
        {
            return BitConverter.ToDouble(SwapEndian(Bytes(8)), 0);
        }

        public string String()
        {
            return Encoding.UTF8.GetString(Bytes(Int16()));
        }

        #endregion

        #region Writing

        public void Write(byte[] buffer, int offset, int size)
        {
            //Console.WriteLine("Wrote {0} bytes.", buffer.Count());
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
