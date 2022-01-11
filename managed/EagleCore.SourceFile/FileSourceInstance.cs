using EagleWeb.Common.IO.FileSystem;
using EagleWeb.Common.NetObjects;
using EagleWeb.Common.Radio;
using EagleWeb.Common.Radio.Modules;
using EasyWav;
using System;
using System.Collections.Generic;
using System.Text;

namespace EagleCore.SourceFile
{
    public class FileSourceInstance : EagleModuleSource
    {
        public FileSourceInstance(IEagleObjectManagerLink link, WebFsFileStream stream) : base(link)
        {
            this.stream = stream;
            reader = WavFileReader.OpenWavFile(stream, 65536);

            //Validate
            if (reader.Info.Channels != 2)
                throw new Exception($"This is not an IQ file. Two channels are required, but only had {reader.Info.Channels}!");
        }

        private WebFsFileStream stream;
        private WavFileReader reader;

        public override bool IsReady => reader != null;
        public override float SampleRate => reader.Info.SampleRate;
        public override long CenterFrequency { get; set; }

        public override unsafe int Read(EagleComplex* ptr, int count)
        {
            return reader.Read((float*)ptr, count * 2) / 2;
        }

        public override void Close()
        {
            stream.Close();
            reader = null;
            stream = null;
        }
    }
}
