using EasyWav.Exceptions;
using EasyWav.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyWav
{
    public abstract unsafe class WavFileReader : WavFileBase
    {
        internal WavFileReader(WavHeader info, Stream stream, int bufferSize) : base(info, stream, bufferSize)
        {
        }

        /// <summary>
        /// Opens a WAV file reader on a stream.
        /// </summary>
        /// <param name="stream">The stream to use, starting at position 0.</param>
        /// <param name="info">Information describing the stream.</param>
        /// <param name="bufferSize">The samples/channel to allocate in the internal buffer. This is the max operation size.</param>
        /// <returns></returns>
        public static WavFileReader OpenWavFile(Stream stream, int bufferSize)
        {
            //Read header
            byte[] header = new byte[WavHeader.HEADER_LENGTH];
            stream.Read(header, 0, WavHeader.HEADER_LENGTH);
            if (!WavHeader.Deserialize(header, out WavHeader info))
                throw new WavHeaderInvalidException();

            //Open correct one
            switch (info.BitsPerSample)
            {
                case WavSampleFormat.Float32: return new WavFileReaderFloat32(info, stream, bufferSize);
                case WavSampleFormat.PCM16: return new WavFileReaderPCM16(info, stream, bufferSize);
                case WavSampleFormat.PCM8: return new WavFileReaderPCM8(info, stream, bufferSize);
                default: throw new WavInvalidSampleFormatException();
            }
        }

        /// <summary>
        /// Reads from the file. Returns the samples read.
        /// </summary>
        /// <param name="input">Input buffer to output to.</param>
        /// <param name="sampleCount">SAMPLES to read, regardless of the number of channels.</param>
        public int Read(float* output, int sampleCount)
        {
            //Make sure the operation isn't too large
            EnsureOperation(sampleCount);

            //Read
            sampleCount = ReadFromDisk(sampleCount);

            //Convert samples
            ConvertSamples(output, sampleCount);

            return sampleCount;
        }

        /// <summary>
        /// Converts samples from the internal buffer into output. Size is checked beforehand.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sampleCount"></param>
        protected abstract void ConvertSamples(float* output, int sampleCount);
    }
}
