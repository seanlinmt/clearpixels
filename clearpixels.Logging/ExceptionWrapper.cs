using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace clearpixels.Logging
{
    public class ExceptionWrapper : Exception
    {
        public ExceptionWrapper(string message, StackTrace trace)
        {

        }

        public ExceptionWrapper(string message, Exception ex)
        {

        }
    }
}
