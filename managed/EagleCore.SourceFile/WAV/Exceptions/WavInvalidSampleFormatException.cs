using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWav.Exceptions
{
    public class WavInvalidSampleFormatException : Exception
    {
        public WavInvalidSampleFormatException() : base("The sample format of this file is not supported. It may be readable in other software.")
        {

        }
    }
}
