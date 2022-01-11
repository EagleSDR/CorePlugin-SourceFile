using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyWav.Readers
{
    internal unsafe class WavFileReaderFloat32 : WavFileReader
    {
        public WavFileReaderFloat32(WavHeader info, Stream stream, int bufferSize) : base(info, stream, bufferSize)
        {
            buffer = OpenBuffer<float>();
        }

        private float* buffer;

        protected override void ConvertSamples(float* output, int sampleCount)
        {
            Buffer.MemoryCopy(buffer, output, sampleCount * sizeof(float), sampleCount * sizeof(float));
        }
    }
}
