using EasyWav.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyWav
{
    public abstract class WavFileBase
    {
        internal WavFileBase(WavHeader info, Stream stream, int bufferSize)
        {
            this.info = info;
            this.stream = stream;
            this.bufferSize = bufferSize;
        }

        private readonly WavHeader info;
        private readonly Stream stream;
        private readonly int bufferSize;

        private byte[] buffer;
        private GCHandle bufferHandle;

        /// <summary>
        /// The size of the internal buffer in groups; This is the maximum operation size.
        /// </summary>
        public int BufferSize => bufferSize;

        /// <summary>
        /// WAV file info.
        /// </summary>
        public WavHeader Info => info;

        /// <summary>
        /// Bytes per sample per channel.
        /// </summary>
        protected int BytesPerSample => (int)info.BitsPerSample / 8;

        /// <summary>
        /// Bytes per group. (A group being a sample from each channel)
        /// </summary>
        protected int BytesPerGroup => BytesPerSample * info.Channels;

        /// <summary>
        /// Size of the buffer in SAMPLES, not groups.
        /// </summary>
        protected int BufferSizeSamples => BufferSize * Info.Channels;

        /// <summary>
        /// The file length in groups. (A group being a sample from each channel)
        /// </summary>
        public long Length => (stream.Length - WavHeader.HEADER_LENGTH) / BytesPerGroup;

        /// <summary>
        /// The file position in groups. (A group being a sample from each channel)
        /// </summary>
        public long Position
        {
            get => (stream.Position - WavHeader.HEADER_LENGTH) / BytesPerGroup;
            set => stream.Position = (BytesPerGroup * value) + WavHeader.HEADER_LENGTH;
        }

        /// <summary>
        /// Allocates and opens a buffer of size [BufferSize * Info.Channels * sizeof(T)]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected unsafe T* OpenBuffer<T>() where T : unmanaged
        {
            //If the buffer is already initialized, abort
            if (buffer != null)
                throw new Exception("Buffer is already initialized.");

            //Allocate
            buffer = new byte[BufferSize * Info.Channels * sizeof(T)];
            bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            return (T*)bufferHandle.AddrOfPinnedObject();
        }

        /// <summary>
        /// Ensures an operation is OK, otherwise throwing an error.
        /// </summary>
        /// <param name="sampleCount"></param>
        protected void EnsureOperation(int sampleCount)
        {
            if (sampleCount > BufferSizeSamples)
                throw new WavOperationTooBigException(sampleCount, BufferSizeSamples);
        }

        /// <summary>
        /// Reads sampleCount samples from the disk from the internal buffer. Returns the samples read.
        /// </summary>
        /// <param name="sampleCount"></param>
        protected int ReadFromDisk(int sampleCount)
        {
            return stream.Read(buffer, 0, sampleCount * BytesPerSample) / BytesPerSample;
        }

        /// <summary>
        /// Writes sampleCount samples to the disk from the internal buffer.
        /// </summary>
        /// <param name="sampleCount"></param>
        protected void WriteToDisk(int sampleCount)
        {
            stream.Write(buffer, 0, sampleCount * BytesPerSample);
        }
    }
}
