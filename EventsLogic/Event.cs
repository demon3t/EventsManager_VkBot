using EventsLogic.HeplerInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventsLogic
{
    public class Event : IListDataTime
    {
        public static List<Event> ActualEvents = new List<Event>();

        #region свойства

        public bool IsCreated { get; set; }
        public string? PersonCreated { get; set; }
        public int? Id { get; set; }
        public bool? IsActual { get; set; }
        public string? Name { get; set; }
        public string? Place { get; set; }
        public int? Seats { get; set; }
        public int? _Seats { get; set; }
        public string? Describe { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        private DateTime RemindTime { get { return StartTime.AddHours(-1); } }

        #endregion

        #region конструкторы
        private Event(string personCreated)
        {
            PersonCreated = personCreated;
            IsCreated = true;
            IsActual = true;
            StartTime = new DateTime(0001, 1, 1, 0, 0, 2);
            StartTime = new DateTime(0001, 1, 1, 0, 0, 1);
            Seats = 0;
        }
        public Event()
        {

        }

        #endregion


        public static void CreateEvent(string personCreated) => ActualEvents.Add(new Event(personCreated));
        public static void RemoveEvent(string personCreated) => ActualEvents.Remove(ActualEvents.Find(x => x.PersonCreated == personCreated));

        public bool CheackReady()
        {
            if (string.IsNullOrWhiteSpace(Name)) return false;
            if (StartTime < DateTime.Now) return false;
            if (EndTime < StartTime) return false;
            if (Seats < 1) return false;
            return true;
        }






        public static bool operator ==(Event a, Event b) => a.Id == b.Id;
        public static bool operator !=(Event a, Event b) => a.Id != b.Id;
        public override bool Equals(object obj)
        {
            if (obj is Event) return this.Id == ((Event)obj).Id;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        public DateTime SortDateTime()
        {
            return StartTime;
        }
    }
}
