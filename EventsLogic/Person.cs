using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic
{
    public class Person
    {

        public static List<Person> Admins = new List<Person>();

        public string Id { get; set; } = string.Empty;
        public string? SurName { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }

        public static bool operator ==(Person a, Person b)
        {
            return a.Id == b.Id;
        }
        public static bool operator !=(Person a, Person b)
        {
            return a.Id != b.Id;
        }
        public override bool Equals(object obj)
        {
            if (obj is Person) return this.Id == ((Person)obj).Id;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
