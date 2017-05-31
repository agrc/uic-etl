using System;

namespace uic_etl.services
{
    internal class DebugService
    {
        private readonly bool _verbose;

        internal DebugService(bool verbose)
        {
            _verbose = verbose;
        }

        internal void AlwaysWrite(string format, params object[] args)
        {
            Console.Write(" # ");
            Console.Write(format, args);

            if (format.EndsWith("."))
            {
                Console.WriteLine();
                return;
            }

            Console.Write("...");
            Console.WriteLine();
        }

        internal void Write(string format, params object[] args)
        {
            if (!_verbose)
            {
                return;
            }

            AlwaysWrite(format, args);
        }
    }
}