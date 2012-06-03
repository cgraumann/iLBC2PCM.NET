using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using CAFReading;
using iLBC;

namespace iLBCTest
{
    class TestConverter
    {

        static void Main(string[] args)
        {
            String source = "record_ilbc.caf";
            String output = "out.wav";
            String blockSize = "30";

            const int BLOCKL_MAX = 240;
            const int BLOCKL_20MS = 160;
            const int BLOCKL_30MS = 240;
            const int NO_OF_WORDS_20MS = 19;
            const int NO_OF_WORDS_30MS = 25;
            int mode = 0;
            if (blockSize == "20") mode = 20;
            else if (blockSize == "30") mode = 30;

            CAFReader cafReader = new CAFReader(source);
            List<short[]> frames = new List<short[]>();

            byte[] audioBytes = cafReader.GetAudioDataBytes();
            short[] audioShorts = new short[audioBytes.Length / 2];

            for (int i = 0, j=0; i < audioShorts.Length; i++, j+=2)
            {
                // Combine to bytes into one short
                short tmp = (short)(((Int16)(audioBytes[j]) << 8) + audioBytes[j + 1]);
                // Swap Endian Mode; The following line is for testing purposes and can be (un)commented as needed
                tmp =  CAFReader.SwapInt16(tmp);
                audioShorts[i] = tmp;
            }


            // Packing the frames with the correct number of words per frame
            int pos = 0;
            int frameCount = audioShorts.Length / (mode == 20 ? NO_OF_WORDS_20MS : NO_OF_WORDS_30MS);
            for (int i = 0; i < frameCount; i++)
            {
                short[] tmp = new short[mode == 20 ? NO_OF_WORDS_20MS : NO_OF_WORDS_30MS];
                for (int j = pos, k = 0; k < tmp.Length; j++, k++)
                {
                    tmp[k] = audioShorts[j];
                    pos++;
                }
                frames.Add(tmp);
            }


            if ((mode > 0))
            {
                ilbc_decoder decoder = new ilbc_decoder(mode, 1);

                bool empty = true;
                short[] decodedBlock = new short[BLOCKL_MAX];
                List<byte> decodedData = new List<byte>();

                System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
                stop.Start();
                short[] data = new short[mode == 20 ? NO_OF_WORDS_20MS : NO_OF_WORDS_30MS];

                foreach (short[] s in frames)
                {
                    short decoded = 0;
                    

                    decoded = decoder.decode(decodedBlock, s, 1);


                    for (short k = 0; k < decoded; k++)
                    {
                        short tmp = decodedBlock[k];
                        if (tmp != 0) empty = false;
                        // Swap Endian Mode; The following line is for testing purposes and can be (un)commented as needed
                        tmp =  CAFReader.SwapInt16(tmp);
                        decodedData.Add((byte)(tmp >> 8));
                        decodedData.Add((byte)(tmp & 0xff));
                    }
                }

                stop.Stop();
                System.Console.WriteLine();
                System.Console.WriteLine("Decoded data with duration: {0}", stop.Elapsed);
                System.Console.WriteLine("Decoded data is empty: {0}", empty);


                byte[] waveData = new byte[decodedData.Count];
                int i = 0;

                foreach (byte b in decodedData)
                {
                    waveData[i] = b;
                    i++;
                }

                // WRITE TO WAV

                WAVWriter wave = new WAVWriter();
                wave.SampleRate = 8000;
                wave.NumChannels = 1;
                wave.BitsPerSample = 16;
                wave.ByteRate = 16000;
                wave.FileSize = waveData.Length + 36; // TODO
                wave.BlockAlign = 2;
                wave.DataSize = waveData.Length; //TODO
                wave.Data = waveData; // TODO
                wave.StoreWave(output);

                Console.WriteLine("Press enter to quit ...");
                Console.Read(); // just to keep this freakin console on view... Crappy.
                return;
            }
        }
    }
}
