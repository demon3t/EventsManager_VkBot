using EventsLogic.DatabaseRequest;
using EventsLogic.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsLogic.Basic
{
    public class Client
    {
        public static List<Client> Admins = new List<Client>();

        #region свойства

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _id;

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                ClientDatabase.UserSetParams(id: _id, surname: _surname);
            }
        }
        private string _surname;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                ClientDatabase.UserSetParams(id: _id, name: _name);
            }
        }
        private string _name;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                ClientDatabase.UserSetParams(id: _id, admin: _isAdmin);
            }
        }
        private bool _isAdmin;

        public int Major
        {
            get { return _major; }
            set
            {
                _major = value;
                ClientDatabase.UserSetParams(id: _id, major: _major);
            }
        }
        private int _major;

        public int Minor
        {
            get { return _minor; }
            set
            {
                _minor = value;
                ClientDatabase.UserSetParams(id: _id, minor: _minor);
            }
        }
        private int _minor;
        public int TempMinor;

        #endregion

        #region конструкторы

        public Client(string id, string name = "NaN", string surname = "NaN")
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
            return int.Parse(Id) ^ int.Parse(Id);
        }

        #endregion
    }
}
