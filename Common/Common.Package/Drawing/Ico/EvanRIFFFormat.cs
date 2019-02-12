using System;
using System.Collections.Generic;
using System.IO;

namespace Common.Package.Drawing.Ico
{
    public class EvanRIFFFormat
    {
        public Chunk MasterChunk = null;

        private void AddAllChunks(List<Chunk> list, Chunk chunk, int ChunkID)
        {
            if (chunk.Children != null)
            {
                for (int i = 0; i < chunk.Children.Count; i++)
                {
                    if (chunk.Children[i].ID == ChunkID)
                    {
                        list.Add(chunk.Children[i]);
                    }
                    this.AddAllChunks(list, chunk.Children[i], ChunkID);
                }
            }
        }

        public List<Chunk> FindAllChunks(int ChunkID)
        {
            List<Chunk> list = new List<Chunk>();
            this.AddAllChunks(list, this.MasterChunk, ChunkID);
            return list;
        }

        public Chunk FindFirstChunk(int ChunkID)
        {
            if (this.MasterChunk == null)
            {
                return null;
            }
            return this.FindInChunk(this.MasterChunk, ChunkID);
        }

        private Chunk FindInChunk(Chunk chunk, int ChunkID)
        {
            if (chunk.Children != null)
            {
                for (int i = 0; i < chunk.Children.Count; i++)
                {
                    if (chunk.Children[i].ID == ChunkID)
                    {
                        return chunk.Children[i];
                    }
                    Chunk chunk2 = this.FindInChunk(chunk.Children[i], ChunkID);
                    if (chunk2 != null)
                    {
                        return chunk2;
                    }
                }
            }
            return null;
        }

        private void GetLISTChildChunks(Chunk chnk, Stream s)
        {
            long position = s.Position;
            s.Seek(chnk.StreamOffset + 12, SeekOrigin.Begin);
            chnk.Children = new List<Chunk>();
            while (s.Position < ((chnk.StreamOffset + chnk.Size) + 8))
            {
                Chunk item = new Chunk {
                    StreamOffset = s.Position,
                    ID = EOStreamUtility.ReadInt(s),
                    Size = EOStreamUtility.ReadInt(s)
                };
                chnk.Children.Add(item);
                s.Seek((long)item.Size, SeekOrigin.Current);
            }
            s.Seek(position, SeekOrigin.Begin);
        }

        public int InitFromStream(Stream s)
        {
            long position = s.Position;
            int num2 = 0x46464952;
            int num3 = EOStreamUtility.ReadInt(s);
            if (num3 != num2)
            {
                return -1;
            }
            this.MasterChunk = new Chunk();
            this.MasterChunk.ID = num3;
            this.MasterChunk.StreamOffset = position;
            this.MasterChunk.Size = EOStreamUtility.ReadInt(s);
            this.MasterChunk.HeaderID = EOStreamUtility.ReadInt(s);
            this.MasterChunk.Children = new List<Chunk>();
            int num4 = 0x5453494c;
            while ((s.Position < ((position + this.MasterChunk.Size) + 8)) && (s.Position < s.Length))
            {
                Chunk chnk = new Chunk {
                    StreamOffset = s.Position,
                    ID = EOStreamUtility.ReadInt(s),
                    Size = EOStreamUtility.ReadInt(s)
                };
                if (chnk.ID == num4)
                {
                    chnk.HeaderID = EOStreamUtility.ReadInt(s);
                    this.GetLISTChildChunks(chnk, s);
                }
                this.MasterChunk.Children.Add(chnk);
                if (chnk.ID == num4)
                {
                    s.Seek((long)(chnk.Size - 4), SeekOrigin.Current);
                }
                else
                {
                    s.Seek((long)chnk.Size, SeekOrigin.Current);
                }
            }
            return 0;
        }

        public class Chunk
        {
            public List<EvanRIFFFormat.Chunk> Children;
            public int HeaderID = 0;
            public int ID = 0;
            public int Size = 0;
            public long StreamOffset = 0;

            public long DataOffset()
            {
                return (this.StreamOffset + 8);
            }

            public string GetStringHeaderID()
            {
                if (this.HeaderID == 0)
                {
                    return null;
                }
                return this.IntToString(this.HeaderID);
            }

            public string GetStringID()
            {
                return this.IntToString(this.ID);
            }

            private string IntToString(int i)
            {
                return new string(new char[] { (char)(i & 0xff), (char)((i >> 8) & 0xff), (char)((i >> 0x10) & 0xff), (char)((i >> 0x18) & 0xff) });
            }
        }
    }

}