using System;
using System.Collections.Generic;
using System.Text;

namespace PartialZip.Exceptions
{
    public class PartialZipNotSupportedException : Exception
    {
        public PartialZipNotSupportedException(string msg) : base(msg) { }
    }
}
