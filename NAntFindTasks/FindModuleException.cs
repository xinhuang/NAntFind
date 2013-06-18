using System;

namespace NAntFind
{
    internal class FindModuleException : FindException
    {
        public FindModuleException(string message)
            : base(message)
        {
        }
    }
}