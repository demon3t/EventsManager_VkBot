using System;
using System.Diagnostics;
using System.Timers;

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
        private string? Path;
        private string? Id;
        private bool conloleWrite = true;
        private bool messageWrite = false;
        private bool fileWrite = false;


        public Loger(string path)
        {
            Path = path;
            fileWrite = true;
        }

        public Loger(string id, bool messagewrite)
        {
            Id = id;
            messageWrite = messagewrite;
        }

        public Loger(string path, string id, bool messagewrite)
        {
            Path = path;
            fileWrite = true; 
            Id = id;
            messageWrite = messagewrite;
        }

        public void Recording(Stopwatch sw)
        {
            sw.Stop();
        }
    }
}