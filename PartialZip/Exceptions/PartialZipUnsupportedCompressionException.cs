using System;
using System.Collections.Generic;
using System.Text;

namespace PartialZip.Exceptions
{
    public class PartialZipUnsupportedCompressionException : Exception
    {
        public PartialZipUnsupportedCompressionException(string msg) : base(msg) { }
    }
}
