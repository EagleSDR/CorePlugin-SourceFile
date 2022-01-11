using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWav.Exceptions
{
    public class WavHeaderInvalidException : Exception
    {
        public WavHeaderInvalidException() : base("The WAV file has an invalid header and cannot be opened.")
        {

        }
    }
}
