using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAFReading
{

    


    class CAFReader
    {

        private String fileName;
        private CAFFileHeader fileHeader;
        private CAFChunkHeader descHeader;
        private CAFAudioFormat descAudioFormat;

        public CAFReader(String fileName)
        {
            this.fileName = fileName;
         }

        private void ReadFile()
        {
            BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));

            // File Header
            fileHeader = new CAFFileHeader(reader.ReadUInt32(),reader.ReadUInt16(),reader.ReadUInt16());
            
            // Description Chunk
            descHeader = new CAFChunkHeader(reader.ReadUInt32(), reader.ReadInt64());
            descAudioFormat = new CAFAudioFormat();
            descAudioFormat.SampleRate = reader.ReadDouble();
            descAudioFormat.FormatID = reader.ReadUInt32();
            descAudioFormat.FormatFlags = reader.ReadUInt32();
            descAudioFormat.BytesPerPacket = reader.ReadUInt32();
            descAudioFormat.FramesPerPacket = reader.ReadUInt32();
            descAudioFormat.ChannelsPerFrame = reader.ReadUInt32();
            descAudioFormat.BitsPerChannel = reader.ReadUInt32();

            // Following Chunks
            String type = CAFReader.UIntToString(reader.ReadUInt32());

            Console.WriteLine("Type: {0}\tVersion: {1}\tFlags: {2}", fileHeader.FileTypeAsString(), fileHeader.FileVersion, fileHeader.FileFlags);
            Console.WriteLine("Chunk Type: {0}\tChunk Size: {1}", descHeader.chunkTypeAsString(), descHeader.ChunkSize);
            Console.WriteLine("Sample rate: {0}\tFormatID: {1}\tFormat flags: {2}\tBytes per Packet: {3}\tFrames per Packet: {4}\tChannels per Frame: {5}\tBits per Channel: {6}\t", descAudioFormat.SampleRate, descAudioFormat.FormatIDAsString(), descAudioFormat.FormatFlags, descAudioFormat.BytesPerPacket, descAudioFormat.FramesPerPacket, descAudioFormat.ChannelsPerFrame, descAudioFormat.BitsPerChannel);
      

            reader.Close();
        }

        private bool FindDataChunk(out long dataPos, out long dataSize)
        {
            bool done = false;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[12]; // 12 bytes = Size of Chunk Header
            UInt32 chunkType = 0, chunkSize = 0;
            dataPos = 0; dataSize = 0;
            UInt32 dataSpecifier = BitConverter.ToUInt32(CAFReader.StringToByteArray("data"),0);

            long bytesRead = stream.Seek(8, SeekOrigin.Begin); // skip file header
            while (!done && bytesRead > 0)
            {
                bytesRead = stream.Read(buffer, 0, 12);
                chunkType = ((UInt32)(buffer[3]) << 24) + ((UInt32)(buffer[2]) << 16) + ((UInt32)(buffer[1]) << 8) + buffer[0];
                Console.WriteLine("Chunktype: {0}", CAFReader.ByteArrayToString(BitConverter.GetBytes(chunkType)));
                if (chunkType == dataSpecifier)
                {
                        dataPos = stream.Position+4; // skip edits
                        dataSize = ((UInt32)(buffer[8]) << 24) + ((UInt32)(buffer[9]) << 16) + ((UInt32)(buffer[10]) << 8) + buffer[11];
                        dataSize -= 4; // edits included in size
                        done = true;
                } else {
                        chunkSize = ((UInt32)(buffer[8]) << 24) + ((UInt32)(buffer[9]) << 16) + ((UInt32)(buffer[10]) << 8) + buffer[11];
                        stream.Seek(chunkSize, SeekOrigin.Current);
                }
            }


            stream.Close();
            return done;
        }

        public byte[] GetAudioDataBytes()
        {
            byte[] data = null;
            long dataPos, dataSize;
            if (this.FindDataChunk(out dataPos, out dataSize))
            {
                data = new byte[dataSize];
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                stream.Seek(dataPos, SeekOrigin.Begin);
                int actPos = 0;
                while (dataSize > int.MaxValue)
                {
                    dataSize -= int.MaxValue;
                    stream.Read(data, actPos, int.MaxValue);
                    actPos += int.MaxValue;
                }
                stream.Read(data,actPos, (int)dataSize);
            }
            return data;
        }

        public short[] GetAudioDataAsShortArray()
        {
            short[] data = null;
            long dataPos, dataSize;
            if (this.FindDataChunk(out dataPos, out dataSize))
            {
                data = new short[dataSize/2];
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                stream.Seek(dataPos, SeekOrigin.Begin);
                BinaryReader reader = new BinaryReader(stream);
                for(int i=0;i<data.Length;i++)
                {
                    data[i] = reader.ReadInt16();
                }
                
            }
            return data;
        }

        public static short SwapInt16(short v)
        {

            return (short)(((v & 0xff) << 8) | ((v >> 8) & 0xff));

        }

        protected internal static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        protected internal static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }

        protected internal static string UIntToString(UInt32 code)
        {
            return ByteArrayToString(BitConverter.GetBytes(code));
        }

        protected internal static int StringToInt(String str)
        {
            return BitConverter.ToInt32(StringToByteArray(str),0);
        }
    }
}
