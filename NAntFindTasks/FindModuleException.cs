using System;

namespace NAntFind
{
    internal class FindModuleException : Exception
    {
        public FindModuleException(string message)
            : base(message)
        {
        }
    }
}