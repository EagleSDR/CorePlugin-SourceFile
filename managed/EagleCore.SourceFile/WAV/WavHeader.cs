using System;
using System.Text;

namespace EasyWav
{
    public class WavHeader
    {
        public const int HEADER_LENGTH = 44;

        public WavHeader(short channels, int sampleRate, WavSampleFormat bitsPerSample, int dataLength)
        {
            Channels = channels;
            SampleRate = sampleRate;
            BitsPerSample = (WavSampleFormat)bitsPerSample;
            DataLength = dataLength;
        }

        public short Channels { get; set; }
        public int SampleRate { get; set; }
        public WavSampleFormat BitsPerSample { get; set; }
        public int DataLength { get; set; }

        public byte[] Serialize()
        {
            //Allocate
            byte[] buffer = new byte[HEADER_LENGTH];
            int offset = 0;

            //Calculate
            short blockAlign = (short)(Channels * ((short)BitsPerSample / 8));
            int avgBytesPerSec = SampleRate * (int)blockAlign;

            //Write
            WriteTag(buffer, ref offset, "RIFF");
            WriteSignedInt(buffer, ref offset, DataLength - HEADER_LENGTH + 8); //Length
            WriteTag(buffer, ref offset, "WAVE");
            WriteTag(buffer, ref offset, "fmt ");
            WriteSignedInt(buffer, ref offset, 16);
            WriteSignedShort(buffer, ref offset, 1); //Format tag
            WriteSignedShort(buffer, ref offset, Channels);
            WriteSignedInt(buffer, ref offset, SampleRate);
            WriteSignedInt(buffer, ref offset, avgBytesPerSec);
            WriteSignedShort(buffer, ref offset, blockAlign);
            WriteSignedShort(buffer, ref offset, (short)BitsPerSample);
            WriteTag(buffer, ref offset, "data");
            WriteSignedInt(buffer, ref offset, DataLength); //Length

            return buffer;
        }

        public static bool Deserialize(byte[] header, out WavHeader info)
        {
            //Parse
            int pos = 0;
            string fileTag = ReadWavTag(header, ref pos);
            int fileLen = ReadWavInt32(header, ref pos);
            string wavTag = ReadWavTag(header, ref pos);
            string fmtTag = ReadWavTag(header, ref pos);
            ReadWavInt32(header, ref pos); //Unknown, 16
            short formatTag = ReadWavInt16(header, ref pos);
            short channels = ReadWavInt16(header, ref pos);
            int fileSampleRate = ReadWavInt32(header, ref pos);
            int avgBytesPerSec = ReadWavInt32(header, ref pos);
            short blockAlign = ReadWavInt16(header, ref pos);
            short bitsPerSample = ReadWavInt16(header, ref pos);
            string dataTag = ReadWavTag(header, ref pos);
            int dataLen = ReadWavInt32(header, ref pos);

            //Validate
            if (fileTag != "RIFF" || wavTag != "WAVE" || fmtTag != "fmt " || dataTag != "data")
            {
                info = null;
                return false;
            }

            //Create
            info = new WavHeader(channels, fileSampleRate, (WavSampleFormat)bitsPerSample, dataLen);
            return true;
        }

        // UTIL

        private static void WriteTag(byte[] buffer, ref int offset, string tag)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(tag);
            WriteBytes(bytes, buffer, ref offset);
        }

        private static void WriteSignedInt(byte[] buffer, ref int offset, int value)
        {
            WriteEndianBytes(BitConverter.GetBytes(value), buffer, ref offset);
        }

        private static void WriteSignedShort(byte[] buffer, ref int offset, short value)
        {
            WriteEndianBytes(BitConverter.GetBytes(value), buffer, ref offset);
        }

        private static void WriteEndianBytes(byte[] data, byte[] buffer, ref int offset)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);
            WriteBytes(data, buffer, ref offset);
        }

        private static void WriteBytes(byte[] src, byte[] data, ref int offset)
        {
            Array.Copy(src, 0, data, offset, src.Length);
            offset += src.Length;
        }

        private static string ReadWavTag(byte[] header, ref int offset)
        {
            string v = Encoding.ASCII.GetString(header, offset, 4);
            offset += 4;
            return v;
        }

        private static int ReadWavInt32(byte[] header, ref int offset)
        {
            int v = BitConverter.ToInt32(header, offset);
            offset += 4;
            return v;
        }

        private static short ReadWavInt16(byte[] header, ref int offset)
        {
            short v = BitConverter.ToInt16(header, offset);
            offset += 2;
            return v;
        }
    }
}
