using System;
using System.Collections.Generic;
using System.Text;

namespace PartialZip.Exceptions
{
    public class PartialZipFileNotFoundException : Exception
    {
        public PartialZipFileNotFoundException(string msg) : base(msg) { }
    }
}
