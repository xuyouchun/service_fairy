using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using Common.Contracts;
using System.Collections;

namespace Common.Package.Storage
{
	partial class StreamTableStorage_V1
	{
        class Reader
        {
            public Reader(Stream stream)
            {
                _stream = stream;
            }

            private readonly Stream _stream;
            private StreamTableHeaderMetadata _headerMetadata;
            private StreamTableBuffer _bodyBuffer;
            private StreamTableMetadata[] _tableMetadata;
            private StreamTableHeapReader _heapReader;

            public StreamTable[] Read(out StreamTableHeaderInfo headerInfo)
            {
                _ReadHeader(out _headerMetadata, out _bodyBuffer, out _heapReader);
                _tableMetadata = _ReadTables(_headerMetadata.TableCount);

                headerInfo = _headerMetadata.ToHeaderInfo();
                return _tableMetadata.ToArray(
                    ti => new ReadOnlyStreamTable(ti, _bodyBuffer, _heapReader, _headerMetadata.IndexType));
            }

            // 读取表元数据
            private StreamTableMetadata[] _ReadTables(int tableCount)
            {
                StreamTableMetadata[] tableMetadatas = new StreamTableMetadata[tableCount];
                for (int k = 0; k < tableCount; k++)
                {
                    StreamTableMetadata ti = new StreamTableMetadata() {
                        TableName = _heapReader.ReadString(_bodyBuffer),
                        ColumnCount = _bodyBuffer.ReadInt32(),
                        RowCount = _bodyBuffer.ReadInt32(),
                        RowLength = _bodyBuffer.ReadInt32(),
                        MetadataOffset = _bodyBuffer.ReadInt32(),
                        DataOffset = _bodyBuffer.ReadInt32(),
                        Desc = _heapReader.ReadString(_bodyBuffer)
                    };

                    tableMetadatas[k] = ti;
                }

                return tableMetadatas;
            }

            // 读取表头
            private void _ReadHeader(out StreamTableHeaderMetadata metadata, out StreamTableBuffer bodyBuffer, out StreamTableHeapReader heapReader)
            {
                IndexType heapIndexType = (IndexType)_stream.ReadUInt8();
                int heapOffset = _stream.ReadInt32();
                _ReadBodyAndHeap(heapOffset, heapIndexType, out bodyBuffer, out heapReader);

                DateTime creationTime = bodyBuffer.ReadDateTime();
                int tableCount = bodyBuffer.ReadInt32();
                string name = _heapReader.ReadString(bodyBuffer);
                string desc = _heapReader.ReadString(bodyBuffer);

                metadata = new StreamTableHeaderMetadata() {
                    Version = _version, IndexType = heapIndexType, HeapOffset = heapOffset,
                    CreationTime = creationTime, TableCount = tableCount, Name = name, Desc = desc,
                };
            }

            private void _ReadBodyAndHeap(int heapOffset, IndexType heapIndexType, out StreamTableBuffer bodyBuffer, out StreamTableHeapReader heapReader)
            {
                StreamTableBuffer heapBuffer;
                byte[] buffer = _stream.GetStreamBuffer();
                //MemoryStream ms;
                if (buffer != null)
                {
                    bodyBuffer = new BytesStreamTableBuffer(buffer, 0, heapOffset, _creationTimeOffset);
                    int heapLength = buffer.ReadIndex(heapIndexType, heapOffset);
                    heapBuffer = new BytesStreamTableBuffer(buffer, heapOffset + (int)heapIndexType, heapLength, 0);
                }
                /*else if ((ms = _stream as MemoryStream) != null)
                {
                    bodyBuffer = new MemoryStreamTableBuffer(ms, 0, heapOffset, (int)ms.Position);
                    ms.Seek(heapOffset, SeekOrigin.Begin);
                    int heapLength = ms.ReadIndex(heapIndexType);
                    heapBuffer = new MemoryStreamTableBuffer(ms, heapOffset + (int)heapIndexType, heapLength);
                }*/
                else  // 普通的流
                {
                    byte[] bodyBytes = new byte[heapOffset];
                    _stream.ReadBytes(bodyBytes, _creationTimeOffset, bodyBytes.Length - _creationTimeOffset, true);
                    bodyBuffer = new BytesStreamTableBuffer(bodyBytes, 0, bodyBytes.Length, _creationTimeOffset);
                    int heapLength = _stream.ReadIndex(heapIndexType);
                    byte[] heapBytes = new byte[heapLength];
                    _stream.ReadBytes(heapBytes, 0, heapBytes.Length, true);
                    heapBuffer = new BytesStreamTableBuffer(heapBytes, 0, heapLength, 0);
                }

                heapReader = new StreamTableHeapReader(heapBuffer, heapIndexType);
            }
        }
	}
}
