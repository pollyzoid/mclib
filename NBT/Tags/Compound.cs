using System;
using System.Text;

namespace MCLib.NBT.Tags
{
    [TagId(Enums.Tag.Compound)]
    public class Compound : TagListBase<string>
    {
        #region Overrides of TagBase

        protected override void ReadData(BinaryReaderMC stream)
        {
            Tags.Clear();

            while (true)
            {
                var id = (Enums.Tag)stream.ReadByte();

                if (id == Enums.Tag.End)
                    break;

                try
                {
                    var t = TagFromId(id);
                    t.Read(stream);

                    Tags[t.Name] = t;
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("Unsupported tag {0:x2} found!", id);
                }
            }
        }

        protected override void WriteData(BinaryWriterMC stream)
        {
            foreach(var t in Tags)
            {
                stream.Write((byte) t.Value.Id);
                t.Value.Write(stream);
            }

            stream.Write((byte) Enums.Tag.End);
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString())
                .Append(": " + Tags.Count)
                .AppendLine(" entries")
                .AppendLine("{");

            foreach (var tag in this)
            {
                sb.AppendFormat("\t{0}\n", tag.ToString().Replace("\n", "\n\t"));
            }

            return sb.Append("}").ToString();
        }
    }
}
