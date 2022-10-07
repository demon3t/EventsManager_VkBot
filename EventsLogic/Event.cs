using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventsLogic
{
    public class Event
    {
        public static List<string> InterestUsers = new List<string>();

        public static List<Event> ActualEvents = new List<Event>();

        public List<string> InvolvedUsers = new List<string>();

        #region свойства
        public int Id { get; set; }
        public bool IsActual { get; set; }
        public string? Name { get; set; }
        public string? Place { get; set; }
        public int Count { get; set; }
        public int _Count { get; set; } = 0;
        public string? Describe { get; set; }
        public DateTime FullDataTime { get { return new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second); } }
        public DateTime FullRemindDataTime { get { return new DateTime(Date.Year, Date.Month, Date.Day, RemindTime.Hour, RemindTime.Minute, RemindTime.Second); } }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        private DateTime RemindTime { get { return Time.AddHours(-1); } }

        #endregion



        public event EventHandler<RemindEventArgs>? Remind;
        protected virtual void OnRemind(RemindEventArgs e)
        {
            Volatile.Read(ref Remind)?.Invoke(this, e);
        }
    }

    public class RemindEventArgs : EventArgs
    {
        private readonly Event _event;

        public RemindEventArgs(Event _event)
        {
            this._event = _event;
        }
        public int Id { get { return _event.Id; } }
        public bool IsActual { get { return _event.IsActual; } }
        public string? Name { get { return _event.Name; } }
        public string? Place { get { return _event.Place; } }
        public int Count { get { return _event.Count; } }
        public string? Describe { get { return _event.Describe; } }
        public DateTime Date { get { return _event.Date; } }
        public DateTime Time { get { return _event.Time; } }
    }
}
