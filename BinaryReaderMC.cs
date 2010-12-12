using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCLib
{
    public class BinaryReaderMC : BinaryReader
    {
        #region Constructor

        public BinaryReaderMC(Stream input)
            : base(input, Encoding.UTF8)
        {
        }

        #endregion

        #region Overrides

        public override short ReadInt16()
        {
            return BitConverter.ToInt16(ToLittleEndian(ReadBytes(2)), 0);
        }

        public override int ReadInt32()
        {
            return BitConverter.ToInt32(ToLittleEndian(ReadBytes(4)), 0);
        }

        public override long ReadInt64()
        {
            return BitConverter.ToInt64(ToLittleEndian(ReadBytes(8)), 0);
        }

        public override float ReadSingle()
        {
            return BitConverter.ToSingle(ToLittleEndian(ReadBytes(4)), 0);
        }

        public override double ReadDouble()
        {
            return BitConverter.ToDouble(ToLittleEndian(ReadBytes(8)), 0);
        }

        public override string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes(ReadInt16()));
        }

        #endregion

        #region Helpers

        private static byte[] ToLittleEndian(IEnumerable<byte> bytes)
        {
            return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes.ToArray();
        }

        #endregion
    }
}
