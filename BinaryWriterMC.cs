using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MCLib
{
    public class BinaryWriterMC : BinaryWriter
    {
        #region Constructor

        public BinaryWriterMC(Stream stream)
            : base(stream, Encoding.UTF8)
        {
        }

        #endregion

        #region Overrides

        public override void Write(short value)
        {
            base.Write(ToBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(int value)
        {
            base.Write(ToBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(long value)
        {
            base.Write(ToBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(float value)
        {
            base.Write(ToBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(double value)
        {
            base.Write(ToBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(string value)
        {
            Write((short) Encoding.UTF8.GetByteCount(value));
            base.Write(Encoding.UTF8.GetBytes(value));
        }

        #endregion

        #region Helpers

        private static byte[] ToBigEndian(IEnumerable<byte> bytes)
        {
            return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes.ToArray();
        }

        #endregion
    }
}
