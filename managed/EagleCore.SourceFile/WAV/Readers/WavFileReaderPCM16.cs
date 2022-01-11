using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyWav.Readers
{
    internal unsafe class WavFileReaderPCM16 : WavFileReader
    {
        public WavFileReaderPCM16(WavHeader info, Stream stream, int bufferSize) : base(info, stream, bufferSize)
        {
            buffer = OpenBuffer<short>();
        }

        private short* buffer;

        protected override void ConvertSamples(float* output, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
                output[i] = buffer[i] / 32767.0f;
        }
    }
}
