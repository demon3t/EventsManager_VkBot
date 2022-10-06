using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic
{
    internal class User
    {
        public string? Id { get; set; }
        public string? SurName { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsMake { get; set; }
        public int MakeState { get; set; }
        public bool IsMark { get; set; }
    }
}
