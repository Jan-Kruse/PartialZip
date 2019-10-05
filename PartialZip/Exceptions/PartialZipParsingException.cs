using System;
using System.Collections.Generic;
using System.Text;

namespace PartialZip.Exceptions
{
    public class PartialZipParsingException : Exception
    {
        public PartialZipParsingException(string msg) : base(msg) { }
    }
}
