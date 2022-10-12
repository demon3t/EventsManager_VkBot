using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace EventsLogic
{
    public class Request
    {
        public int Id { get; set; }
        public string Sourse { get; set; }
        public DateTime Time { get; set; }
        public Timer? ProcessingTime { get; set; }
        public string? Description { get; set; }

        public Request(string sourse, DateTime time, string? description = null)
        {
            Sourse = sourse;
            Time = time;
            Description = description;
        }
    }
}
