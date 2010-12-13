namespace MCLib.NBT.Tags
{
    [TagId(Enums.Tag.Byte)]
    public class Byte : TagValueBase<byte>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadByte();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }

    [TagId(Enums.Tag.Short)]
    public class Short : TagValueBase<short>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadInt16();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }

    [TagId(Enums.Tag.Int)]
    public class Int : TagValueBase<int>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadInt32();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }

    [TagId(Enums.Tag.Long)]
    public class Long : TagValueBase<long>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadInt64();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }

    [TagId(Enums.Tag.Float)]
    public class Float : TagValueBase<float>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadSingle();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }

    [TagId(Enums.Tag.Double)]
    public class Double : TagValueBase<double>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadDouble();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }

    [TagId(Enums.Tag.ByteArray)]
    public class ByteArray : TagValueBase<byte[]>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadBytes(stream.ReadInt32());
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value.Length);
            stream.Write(Value);
        }

        #endregion

        public override string ToString()
        {
            return "TAG_" + Id + (!string.IsNullOrEmpty(Name) ? "(\"" + Name + "\")" : "") + ": [" + Value.Length + " bytes]";
        }
    }

    [TagId(Enums.Tag.String)]
    public class String : TagValueBase<string>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Value = stream.ReadString();
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write(Value);
        }

        #endregion
    }
}
