using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventsLogic
{
    public class Event
    {
        public static List<Event> ActualEvents = new List<Event>();

        private Event(string personCreated)
        {
            PersonCreated = personCreated;
            IsCreated = true;
            IsActual = true;
        }

        #region свойства

        public bool IsCreated { get; set; }
        public string PersonCreated { get; set; } 
        public int? Id { get; set; }
        public bool? IsActual { get; set; }
        public string? Name { get; set; }
        public string? Place { get; set; }
        public int? Seats { get; set; }
        public int? _Seats { get; set; }
        public string? Describe { get; set; }
        public DateTime FullDataTime { get { return new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second); } }
        public DateTime FullRemindDataTime { get { return new DateTime(Date.Year, Date.Month, Date.Day, RemindTime.Hour, RemindTime.Minute, RemindTime.Second); } }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        private DateTime RemindTime { get { return Time.AddHours(-1); } }

        #endregion

        public static void CreateEvent(string personCreated)
        {
            ActualEvents.Add(new Event(personCreated));
        }


        public static bool operator ==(Event a, Event b)
        {
            return a.Id == b.Id;
        }
        public static bool operator !=(Event a, Event b)
        {
            return a.Id != b.Id;
        }
        public override bool Equals(object obj)
        {
            if (obj is Event) return this.Id == ((Event)obj).Id;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
