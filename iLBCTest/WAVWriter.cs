using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iLBCTest
{
    class WAVWriter
    {
        public int FileSize {get; set; }
        public string Format { get; set; }
        public int FmtChunkSize { get; set; }
        public int AudioFormat { get; set; }
        public int NumChannels { get; set; }
        public int SampleRate { get; set; }
        public int ByteRate { get; set; }
        public int BlockAlign { get; set; }
        public int BitsPerSample { get; set; }
        public int DataSize { get; set; }

        public byte[] Data { get; set; }

        public WAVWriter()
        {
            Format = "WAVE";
            FmtChunkSize = 16;
            AudioFormat = 1;
        }

        public void StoreWave(string path)
        {
            System.IO.File.Delete(path);
            System.IO.FileStream fs = System.IO.File.Create(path); // zu schreiben Wave Datei öffnen / erstellen
            StoreChunk(fs, "RIFF"); // RIFF Chunk schreiben
            StoreChunk(fs, "fmt "); // fmt Chunk schreiben
            StoreChunk(fs, "data"); // data Chunk schreiben
        }

        private void StoreChunk(System.IO.FileStream fs, string chunkID)
        {
            System.Text.ASCIIEncoding Decoder = new ASCIIEncoding();
            // den Namen in Bytes konvertieren und schreiben
            fs.Write(Decoder.GetBytes(chunkID), 0, 4);

            if (chunkID == "RIFF")
            {
                // im RIFF Chunk, FileSize als Größe und das Audioformat schreiben
                fs.Write(System.BitConverter.GetBytes(FileSize), 0, 4);
                fs.Write(Decoder.GetBytes(Format), 0, 4);
            }
            if (chunkID == "fmt ")
            {
                // beim fmt Chunk die Größe dieses sowie die weiteren kodierten Informationen schreiben
                fs.Write(System.BitConverter.GetBytes(FmtChunkSize), 0, 4);
                fs.Write(System.BitConverter.GetBytes(AudioFormat), 0, 2);
                fs.Write(System.BitConverter.GetBytes(NumChannels), 0, 2);
                fs.Write(System.BitConverter.GetBytes(SampleRate), 0, 4);
                fs.Write(System.BitConverter.GetBytes(ByteRate), 0, 4);
                fs.Write(System.BitConverter.GetBytes(BlockAlign), 0, 2);
                fs.Write(System.BitConverter.GetBytes(BitsPerSample), 0, 2);
            }
            if (chunkID == "data")
            {
                // beim data Chunk die Größe des Datenblocks als Größenangabe schreiben
                fs.Write(System.BitConverter.GetBytes(DataSize), 0, 4);
                // dann die einzelnen Amplituden, wie beschrieben Sample für Sample mit jeweils allen
                // Audiospuren, schreiben
                // CAVE: z.Zt. werden Channels ignoriert!!!!
                int count = 0;
                for (int i = 0; i < Data.Length; i++)
                {
                        fs.WriteByte(Data[i]);
                        count++;                
                }
                Console.WriteLine("Durchlaeufe: {0}", count);
            }
        }

    }
}
