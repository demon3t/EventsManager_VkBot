using vkBot.Request;
using EventsLogic.HeplerInterfaces;
using static vkBot.Request.EventRequest;
using System;
using System.Collections.Generic;
using System.Text;
using vkBot.HelperElements;

namespace EventsLogic.Basic
{
    public class Event : IListDataTime
    {
        public static List<Event> ActualEvents = new List<Event>();

        public static SortedList<long, int> OnCreatedEvents = new SortedList<long, int>();

        #region свойства

        public int Id { get; set; }
        public long Author { get; set; }
        public DateTime CreateTime { get; set; }
        public int Occup { get; set; }

        public bool IsActual
        {
            get { return _isActual; }
            set
            {
                _isActual = value;
                EventRequest.Set(Id, isActual: value);
            }
        }
        private bool _isActual;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                EventRequest.Set(Id, name: value);
            }
        }
        private string _name;

        public string Place
        {
            get { return _place; }
            set
            {
                _place = value;
                EventRequest.Set(Id, place: value);
            }
        }
        private string _place;

        public int Seats
        {
            get { return _seats; }
            set
            {
                _seats = value;
                EventRequest.Set(Id, seats: value);
            }
        }
        private int _seats;

        public string Describe
        {
            get { return _describe; }
            set
            {
                _describe = value;
                EventRequest.Set(Id, describe: value);
            }
        }
        private string _describe;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                EventRequest.Set(Id, startTime: value);
            }
        }
        private DateTime _startTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                EventRequest.Set(Id, endTime: value);
            }
        }
        private DateTime _endTime;

        #endregion

        #region конструкторы

        public Event(long author)
        {
            Author = author;
        }

        #endregion


        public bool Ready()
        {
            if (_name.IsNaN()) return false;
            if (StartTime < DateTime.Now) return false;
            if (EndTime < StartTime) return false;
            if (Seats < 1) return false;
            return true;
        }


        public static Event GetEventFromActual(string insexStr)
        {
            if (int.TryParse(insexStr, out int index))
            {
                if (index - 1 < 0 || index - 1 >= ActualEvents.Count) return null;
                return ActualEvents[index - 1];
            }
            return null;
        }

        public static bool operator ==(Event a, Event b)
        {
            if (a is Event && b is null) return false;
            if (a is null && b is Event) return false;
            if (a is Event && b is Event) return a.Id == b.Id;
            return false;
        }
        public static bool operator !=(Event a, Event b)
        {
            if (a is Event && b is null) return true;
            if (a is null && b is Event) return true;
            if (a is Event && b is Event) return a.Id != b.Id;
            return true;
        }

        #region override

        public override bool Equals(object obj)
        {
            if (obj is Event) return ((Event)obj).Id == Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id ^ Id;
        }
        public override string ToString()
        {
            return
                $"{Environment.NewLine}" +
                $"{(string.IsNullOrWhiteSpace(_name) ? "" : _name + Environment.NewLine)}" +
                $"C {_startTime} {Environment.NewLine}" +
                $"до {_endTime} {Environment.NewLine}" +
                $"нужны {_seats - Occup} волонтёра(ов) из {_seats + Environment.NewLine}" +
                $"{(string.IsNullOrWhiteSpace(_name) ? "" : "Место проведения " + _name)}";
        }

        #endregion

        public DateTime SortDateTime() => StartTime;
    }
}
