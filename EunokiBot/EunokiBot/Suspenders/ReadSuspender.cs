using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot
{
    public class ReadSuspender : IDisposable
    {
        private static int _count = 0;

        public ReadSuspender()
        {
            ++_count;
        }

        public static bool IsSuspended
        {
            get { return _count != 0; }
        }

        public void Dispose()
        {
            --_count;
        }
    }
}
