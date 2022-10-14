using EventsLogic.DatabaseRequest;
using EventsLogic.HeplerInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EventsLogic
{
    public class Event : IListDataTime
    {
        public static List<Event> ActualEvents = new List<Event>();

        public static SortedList<string, int> OnCreatedEvents = new SortedList<string, int>();

        #region свойства

        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateTime { get; set; }
        public int _Seats { get; set; }

        public bool IsActual
        {
            get { return _isActual; }
            set
            {
                _isActual = value;
                EventsDatabase.EventSetParams(Id, isActual: value);
            }
        }
        private bool _isActual;

        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                EventsDatabase.EventSetParams(Id, name: value);
            }
        }
        private string? _name;

        public string? Place
        {
            get { return _place; }
            set
            {
                _place = value;
                EventsDatabase.EventSetParams(Id, place: value);
            }
        }
        private string? _place;

        public int Seats
        {
            get { return _seats; }
            set
            {
                _seats = value;
                EventsDatabase.EventSetParams(Id, seats: value);
            }
        }
        private int _seats;

        public string? Describe
        {
            get { return _describe; }
            set
            {
                _describe = value;
                EventsDatabase.EventSetParams(Id, describe: value);
            }
        }
        private string? _describe;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                EventsDatabase.EventSetParams(Id, startTime: value);
            }
        }
        private DateTime _startTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                EventsDatabase.EventSetParams(Id, endTime: value);
            }
        }
        private DateTime _endTime;

        #endregion

        #region конструкторы

        public Event(string author)
        {
            Author = author;
        }
        public Event() { Author = ""; }

        #endregion


        public bool CheackReady()
        {
            if (string.IsNullOrWhiteSpace(Name)) return false;
            if (StartTime < DateTime.Now) return false;
            if (EndTime < StartTime) return false;
            if (Seats < 1) return false;
            return true;
        }

        public static Event? GetEventFromActual(string? insexStr)
        {
            if (int.TryParse(insexStr, out int index))
            {
                if (index - 1 < 0 || index - 1 >= ActualEvents.Count) return null;
                return ActualEvents[index - 1];
            }
            return null;
        }


        private static bool Nullable(object? obj)
        {
            if (obj == null) return true;
            return false;
        }

        public static bool operator ==(Event a, Event b)
        {
            if (Nullable(a) && !Nullable(b)) return false;
            if (!Nullable(a) && Nullable(b)) return false;
            if (Nullable(a) && Nullable(b)) return true;
            return a.Id == b.Id;
        }

        public static bool operator !=(Event a, Event b)
        {
            if (Nullable(a) && !Nullable(b)) return true;
            if (!Nullable(a) && Nullable(b)) return true;
            if (Nullable(a) && Nullable(b)) return false;
            return a.Id != b.Id;
        }

        public override bool Equals(object obj)
        {
            var @event = obj as Event;
            if (@event != null)
            {
                return @event.Id == this.Id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return
                $"{Environment.NewLine}" +
                $"{(string.IsNullOrWhiteSpace(_name) ? "" : _name + Environment.NewLine)}" +
                $"C {_startTime} {Environment.NewLine}" +
                $"до {_endTime} {Environment.NewLine}" +
                $"нужны {_seats - _Seats} волонтёра(ов) из {_seats + Environment.NewLine}" +
                $"{(string.IsNullOrWhiteSpace(_name) ? "" : "Место проведения " + _name)}";
        }



        public DateTime SortDateTime() => StartTime;
    }
}
