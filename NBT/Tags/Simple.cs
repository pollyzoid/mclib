namespace MCLib.NBT.Tags
{
    [TagId(TagId.Byte)]
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

    [TagId(TagId.Short)]
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

    [TagId(TagId.Int)]
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

    [TagId(TagId.Long)]
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

    [TagId(TagId.Float)]
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

    [TagId(TagId.Double)]
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

    [TagId(TagId.ByteArray)]
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

    [TagId(TagId.String)]
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
