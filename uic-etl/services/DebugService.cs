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

        internal void Write(string format, params object[] args)
        {
            if (!_verbose)
            {
                return;
            }

            Console.Write(" # ");
            Console.WriteLine(format, args);
        } 
    }
}