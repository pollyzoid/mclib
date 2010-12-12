using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCLib.NBT.Tags
{
    public abstract class TagBase
    {
        #region Fields

        public string Name { get; private set; }

        private TagId? _id;

        public TagId Id
        {
            get { return (TagId)(_id ?? (_id = GetType().GetCustomAttributes(false).OfType<TagIdAttribute>().Single().Id)); }
        }

        private static readonly Dictionary<TagId, Func<TagBase>> TagList;

        #endregion

        #region Public methods

        public void Read(BinaryReaderMC stream, bool readName = true)
        {
            if (readName)
            {
                Name = stream.ReadString();
            }

            ReadData(stream);
        }

        public void Write(BinaryWriterMC stream)
        {
            if (!string.IsNullOrEmpty(Name))
                stream.Write(Name);

            WriteData(stream);
        }

        #endregion

        #region Private methods

        protected abstract void ReadData(BinaryReaderMC stream);
        protected abstract void WriteData(BinaryWriterMC stream);

        #endregion

        #region Static methods

        static TagBase()
        {
            TagList = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof (TagBase))
                            && t.GetCustomAttributes(false).OfType<TagIdAttribute>().Any())
                .ToDictionary(
                    t => t.GetCustomAttributes(false).OfType<TagIdAttribute>().Single().Id,
                    t => (Func<TagBase>)(() => (TagBase)Activator.CreateInstance(t)));
        }

        public static TagBase TagFromId(TagId id)
        {
            if (!TagList.ContainsKey(id))
            {
                throw new NotImplementedException(string.Format("Invalid tag 0x{0:X2}", (byte) id));
            }

            return TagList[id]();
        }

        #endregion

        public new virtual string ToString()
        {
            return "TAG_" + Id + (!string.IsNullOrEmpty(Name) ? "(\"" + Name + "\")" : "");
        }
    }

    public abstract class TagValueBase<T> : TagBase
    {
        public T Value { get; set; }

        public override string ToString()
        {
            return base.ToString() + ": " + Value;
        }
    }

    public abstract class TagListBase<T> : TagBase, IEnumerable<TagBase>
    {
        TagBase this[T idx]
        {
            get { return Tags[idx]; }
        }

        private Dictionary<T, TagBase> _tags;

        public Dictionary<T, TagBase> Tags
        {
            get { return _tags ?? (_tags = new Dictionary<T, TagBase>()); }
        }

        #region Implementation of IEnumerable

        public IEnumerator<TagBase> GetEnumerator()
        {
            return Tags.Select(tag => tag.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TagIdAttribute : Attribute
    {
        public TagId Id { get; private set; }

        public TagIdAttribute(TagId id)
        {
            Id = id;
        }
    }

    public enum TagId
    {
        End = 0x00,
        Byte = 0x01,
        Short = 0x02,
        Int = 0x03,
        Long = 0x04,
        Float = 0x05,
        Double = 0x06,
        ByteArray = 0x07,
        String = 0x08,
        List = 0x09,
        Compound = 0x0A
    }
}
