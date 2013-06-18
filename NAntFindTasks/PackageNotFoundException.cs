using System;

namespace NAntFind
{
    internal class PackageNotFoundException : FindException
    {
        public PackageNotFoundException(string message)
            : base(message)
        {
        }
    }
}