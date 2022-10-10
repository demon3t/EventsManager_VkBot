namespace Logging
{
    public enum TypeMessage
    {
        Log,
        Info,
        Warning,
        Error
    }

    public class Loger
    {

        public string Path { private get; set; }

        public Loger(string path)
        {
            Path = path;
        }
    }
}