using System;

namespace NAntFind
{
    public class FindException : Exception
    {
        protected FindException(string message)
            : base(message)
        {
        }
    }
}