using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic
{
    public class Person
    {
        public string Id { get; set; } = string.Empty;
        public string? SurName { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
    }
}
