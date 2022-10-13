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

        public static  List<int> OnCreatedEvents = new List<int>();

        #region свойства

        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActual { get; set; }
        public string? Name { get; set; }
        public string? Place { get; set; }
        public int Seats { get; set; }
        public int _Seats { get; set; }
        public string Describe { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        #endregion

        #region конструкторы


        #endregion


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
