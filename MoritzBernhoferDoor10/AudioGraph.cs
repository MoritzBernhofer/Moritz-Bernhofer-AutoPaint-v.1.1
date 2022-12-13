using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoritzBernhoferDoor11
{
    public class MyAudio
    {
        public MyAudio()
        {
            DemoSingleChannel(-1);
        }

        public static string Adio = null;
        public static void DemoSingleChannel(int deviceID)
        {
            var waveIn = new NAudio.Wave.WaveInEvent
            {
                DeviceNumber = deviceID,
                WaveFormat = new NAudio.Wave.WaveFormat(rate: 44100, bits: 16, channels: 1),
                BufferMilliseconds = 20
            };

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            static void WaveIn_DataAvailable(object? sender, NAudio.Wave.WaveInEventArgs e)
            {
                // copy buffer into an array of integers
                Int16[] values = new Int16[e.Buffer.Length / 2];
                Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);

                // determine the highest value as a fraction of the maximum possible value
                float fraction = (float)values.Max() / (1 << 15);

                // print a level meter using the console
                string bar = new('#', (int)(fraction * 70));
                string meter = "[" + bar.PadRight(60, '-') + "]";

                Adio = meter;
            }
        }
    }
}
