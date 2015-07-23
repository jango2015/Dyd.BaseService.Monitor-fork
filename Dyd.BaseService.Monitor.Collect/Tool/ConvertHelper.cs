using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Collect.Tool
{
    public static class ConvertHelper
    {
        public static double ByteToM(ulong bytescount)
        {
            return (double)bytescount / 1024 / 1024;
        }
    }
}
