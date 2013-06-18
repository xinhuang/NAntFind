namespace NAntFind
{
    internal class FileNotFoundException : FindException
    {
        public FileNotFoundException(string message) : base(message)
        {
        }
    }
}