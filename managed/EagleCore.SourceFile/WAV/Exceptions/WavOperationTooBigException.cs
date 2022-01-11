using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWav.Exceptions
{
    public class WavOperationTooBigException : Exception
    {
        public WavOperationTooBigException(int specified, int max) : base($"The requested operation, {specified} samples, exceeded the size of the buffer, {max} samples.")
        {

        }
    }
}
