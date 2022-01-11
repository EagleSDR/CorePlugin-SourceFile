using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyWav.Readers
{
    internal unsafe class WavFileReaderPCM8 : WavFileReader
    {
        public WavFileReaderPCM8(WavHeader info, Stream stream, int bufferSize) : base(info, stream, bufferSize)
        {
            buffer = OpenBuffer<byte>();
        }

        private byte* buffer;

        protected override void ConvertSamples(float* output, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
                output[i] = (buffer[i] - 127.5f) / 127.5f;
        }
    }
}
