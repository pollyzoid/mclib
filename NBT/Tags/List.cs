using System.Text;

namespace MCLib.NBT.Tags
{
    [TagId(TagId.List)]
    public class List : TagListBase<int>
    {
        public TagId Type { get; set; }

        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Type = (TagId)stream.ReadByte();
            var len = stream.ReadInt32();

            Tags.Clear();

            for (var i = 0; i < len; ++i)
            {
                Tags[i] = TagFromId(Type);
                Tags[i].Read(stream, false);
            }
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            stream.Write((byte)Type);
            stream.Write(Tags.Count);

            foreach(var t in Tags)
            {
                t.Value.Write(stream);
            }
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString())
                .Append(": " + Tags.Count + " entries of type TAG_")
                .AppendLine(Type.ToString())
                .AppendLine("{");

            foreach(var tag in this)
            {
                sb.AppendFormat("\t{0}\n", tag.ToString().Replace("\n", "\n\t"));
            }

            return sb.Append("}").ToString();
        }
    }
}