using System.IO;
using System.IO.Compression;
using MCLib.NBT.Tags;
using IOFile = System.IO.File;

namespace MCLib.NBT
{
    public class File
    {
        #region Fields

        public string Path { get; set; }
        public Compound Root { get; private set; }

        #endregion

        #region Constructor

        public File(string path)
        {
            Path = path;
        }

        #endregion

        #region Public methods

        public void Load()
        {
            Load(Path);
        }

        public void Load(string path)
        {
            using(var file = IOFile.OpenRead(path))
            {
                Load(file);
            }
        }

        public void Load(Stream stream)
        {
            using (var decStream = new GZipStream(stream, CompressionMode.Decompress))
            {
                using (var memStream = new MemoryStream((int)stream.Length))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = decStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }

                    LoadInternal(memStream);
                }
            }
        }

        public void Save()
        {
            Save(Path);
        }

        public void Save(string path)
        {
            using(var file = IOFile.OpenWrite(path))
            {
                Save(file);
            }
        }

        public void Save(Stream stream)
        {
            using (var comStream = new GZipStream(stream, CompressionMode.Compress))
            {
                SaveInternal(comStream);
            }
        }

        #endregion

        #region Private methods

        private void LoadInternal(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            var bstream = new BinaryReaderMC(stream);

            if ((Enums.Tag) bstream.ReadByte() != Enums.Tag.Compound) return;

            Root = new Compound();
            Root.Read(bstream);
        }
        
        private void SaveInternal(Stream stream)
        {
            var bstream = new BinaryWriterMC(stream);

            bstream.Write((byte)Enums.Tag.Compound);

            Root.Write(bstream);
        }

        #endregion
    }
}
