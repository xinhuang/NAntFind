using System;

namespace NAntFind
{
    internal class PackageNotFoundException : Exception
    {
        public PackageNotFoundException(string message)
            : base(message)
        {
        }
    }
}