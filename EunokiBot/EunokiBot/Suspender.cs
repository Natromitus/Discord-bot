using System;
using System.Collections.Generic;
using System.Text;

namespace EunokiBot
{
    public class Suspender : IDisposable
    {
        private static int _count = 0;

        public Suspender()
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
