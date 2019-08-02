using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot
{
    class WriteSuspender : IDisposable
    {
        private static int _count = 0;

        public WriteSuspender()
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
