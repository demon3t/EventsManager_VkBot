using EventsLogic;
using EventsLogic.DatabaseRequest;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using VkBotFramework.Models;

namespace vkBot
{
    static internal class StatusLogic
    {
        private enum Major
        {
            FirstOccurrence = 0,
            Normal = 1,
            LookEvents = 2,
            MyEvents = 3,
            CreateEvent = 4,
            CompletEvents = 5,
        }

        private enum Create
        {
            Name = 1,
            Describe = 2,
            StartTime = 3,
            EndTime = 4,
            Seats = 5,
            Place = 6,
        }

        internal static void FirstOccurrence(Person person, MessageReceivedEventArgs e)
        {
            MessageConstructor.FirstOccurrence(person, e);
            UsersDatabase.UserSetParams(person, major: (int)Major.Normal, minor: 0);
        }

        internal static void MainMenu(Person person, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Посмотреть мероприятия":
                    {
                        LookEvents(person, e);
                        return;
                    }
                case "Создать мероприятие":
                    {
                        if (!person.IsAdmin) return;
                        Event.CreateEvent(person.Id);
                        MessageConstructor.CreateEvent(person, e);
                        UsersDatabase.UserSetParams(person, major: (int)Major.CreateEvent, minor: 0);
                        return;
                    }
                case "Мои мероприятия":
                    {
                        MessageConstructor.MyEvents(person, e);
                        UsersDatabase.UserSetParams(person, major: (int)Major.MyEvents, minor: 0);
                        return;
                    }
                case "Завершённые мероприятия":
                    {
                        MessageConstructor.CompletEvents(person, e);
                        UsersDatabase.UserSetParams(person, major: (int)Major.CompletEvents, minor: 0);
                        return;
                    }
                case "Информация":
                    {
                        FirstOccurrence(person, e);
                        return;
                    }
                case "Обо мне":
                    {
                        MessageConstructor.AboutMe(person, e);
                        UsersDatabase.UserSetParams(person, major: (int)Major.Normal, minor: 0);
                        return;
                    }
                default: return;
            }
        }


        #region Посмотреть мероприятия

        internal static void LookEvents(Person person, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Посмотреть мероприятия":
                    {
                        UsersDatabase.UserSetParams(person, major: (int)Major.LookEvents, minor: 0);
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "->":
                    {
                        UsersDatabase.UserSetParams(person, major: (int)Major.LookEvents, minor: person.Minor + 1);
                        person.Minor += 1;
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "<-":
                    {
                        UsersDatabase.UserSetParams(person, major: (int)Major.LookEvents, minor: person.Minor - 1);
                        person.Minor -= 1;
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "Назад":
                    {
                        UsersDatabase.UserSetParams(person, major: (int)Major.LookEvents, minor: 0);
                        MessageConstructor.Back(person, e);
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        #endregion

        #region Создать мероприятие

        internal static void CreateEvent(Person person, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Создать":
                    {
                        var @event = Event.ActualEvents.Find(x => x.PersonCreated == person.Id);
                        MessageConstructor.OnCreateEvent(person, e);
                        @event.PersonCreated = null;
                        EventsDatabase.AddEvent(@event);
                        UsersDatabase.UserSetParams(person, major: (int)Major.Normal, minor: 0);
                        return;
                    }
                case "Название":
                    {
                        MessageConstructor.WaitingParameters_Name(person, e);
                        UsersDatabase.UserSetParams(person, minor: (int)Create.Name);
                        return;
                    }
                case "Опиcание":
                    {
                        MessageConstructor.WaitingParameters_Describe(person, e);
                        UsersDatabase.UserSetParams(person, minor: (int)Create.Describe);
                        return;
                    }
                case "Время начала":
                    {
                        MessageConstructor.WaitingParameters_StartTime(person, e);
                        UsersDatabase.UserSetParams(person, minor: (int)Create.StartTime);
                        return;
                    }
                case "Время конца":
                    {
                        MessageConstructor.WaitingParameters_EndTime(person, e);
                        UsersDatabase.UserSetParams(person, minor: (int)Create.EndTime);
                        return;
                    }
                case "Число волонтёров":
                    {
                        MessageConstructor.WaitingParameters_Seats(person, e);
                        UsersDatabase.UserSetParams(person, minor: (int)Create.Seats);
                        return;
                    }
                case "Место":
                    {
                        MessageConstructor.WaitingParameters_Place(person, e);
                        UsersDatabase.UserSetParams(person, minor: (int)Create.Place);
                        return;
                    }
                case "Назад":
                    {
                        MessageConstructor.Back(person, e);
                        Event.RemoveEvent(person.Id);
                        UsersDatabase.UserSetParams(person, major: (int)Major.Normal, minor: 0);
                        return;
                    }
                default:
                    {
                        SetParamEvent(person, e);
                        return;
                    }
            }
        }
        internal static void SetParamEvent(Person person, MessageReceivedEventArgs e)
        {
            Event @event = Event.ActualEvents.Find(x => x.PersonCreated == person.Id);

            switch ((Create)person.Minor)
            {
                case Create.Name:
                    {
                        @event.Name = e.Message.Text;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
                case Create.Describe:
                    {
                        @event.Describe = e.Message.Text;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
                case Create.StartTime:
                    {
                        if (DateTime.TryParse(e.Message.Text, out DateTime dateTime))
                            @event.StartTime = dateTime;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
                case Create.EndTime:
                    {
                        if (DateTime.TryParse(e.Message.Text, out DateTime dateTime))
                            @event.EndTime = dateTime;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
                case Create.Seats:
                    {
                        if (int.TryParse(e.Message.Text, out int seats))
                            @event.Seats = seats;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
                case Create.Place:
                    {
                        @event.Place = e.Message.Text;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
            }
        }

        #endregion


    }
}
