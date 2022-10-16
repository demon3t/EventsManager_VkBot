using System;
using System.Collections.Generic;
using System.Text;
using vkBot.HelperElements.Classes;
using static vkBot.Request.ClientRequest;

namespace EventsLogic.Basic
{
    public class Client
    {
        public static SortedList<long, Client> Admins = new SortedList<long, Client>();

        #region свойства

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _id;

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                Set(id: _id, surname: _surname);
            }
        }
        private string _surname;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Set(id: _id, name: _name);
            }
        }
        private string _name;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                Set(id: _id, admin: _isAdmin);
            }
        }
        private bool _isAdmin;

        public int Major
        {
            get { return _major; }
            set
            {
                _major = value;
                Set(id: _id, major: _major);
            }
        }
        private int _major;

        public int Minor
        {
            get { return _minor; }
            set
            {
                _minor = value;
                Set(id: _id, minor: _minor);
            }
        }
        private int _minor;
        public int TempMinor;

        #endregion

        #region конструкторы

        public Client(long id, string name = "NaN", string surname = "NaN")
        {
            _id = id;
            _name = name;
            _surname = surname;
        }

        #endregion

        public string About()
        {
            return
                $"{(_name.IsNaN() ? "" : $"Имя: {_name}{Environment.NewLine}")}" +
                $"{(_surname.IsNaN() ? "" : $"Фамилия: {_surname}{Environment.NewLine}")}" +
                $"Администратор: {(_isAdmin ? "да" : "нет")}{Environment.NewLine}" +
                $"Рейтинг: {"пока ничего"}{Environment.NewLine}" +
                $"Что то ещё: {"пока не придумал"}{Environment.NewLine}";
        }


        public static bool operator ==(Client a, Client b)
        {
            if (a is Client && b is Client) return a.Id == b.Id;
            return false;
        }
        public static bool operator !=(Client a, Client b)
        {
            if (a is Client && b is Client) return a.Id != b.Id;
            return true;
        }

        #region override

        public override bool Equals(object obj)
        {
            if (obj is Client) return ((Client)obj).Id == Id;
            return false;
        }
        public override int GetHashCode()
        {
            return (int)_id ^ (int)_id;
        }

        #endregion
    }
}
