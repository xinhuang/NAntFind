namespace NAntFind
{
    class FileNotFoundException:FindException
    {
        public FileNotFoundException(string message) : base(message)
        {
        }
    }
}