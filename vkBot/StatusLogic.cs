using EventsLogic;
using EventsLogic.DatabaseRequest;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using VkBotFramework.Models;
using EventsLogic.HelperClasses;
using System.Linq;

namespace vkBot
{
    static internal class StatusLogic
    {
        private enum Major
        {
            FirstOccurrence = 0,
            Normal = 1,
            LookEvents = 2,
            RequestEvent = 6,

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
            UsersDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
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
                        //возврат если пользователь не админ
                        if (!person.IsAdmin) return;

                        //добавлние нового мероприятия в базу данных
                        EventsDatabase.AddEvent(person.Id);

                        //добавление индекса в лист создаваемого мероприятия
                        Event.OnCreatedEvents.Add(person.Id, EventsDatabase.SetLastIndex());

                        MessageConstructor.CreateEvent(person, e);
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.CreateEvent, minor: 0);
                        return;
                    }
                case "Мои мероприятия":
                    {
                        MessageConstructor.MyEvents(person, e);
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.MyEvents, minor: 0);
                        return;
                    }
                case "Завершённые мероприятия":
                    {
                        MessageConstructor.CompletEvents(person, e);
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.CompletEvents, minor: 0);
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
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
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
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents, minor: 0);
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "->":
                    {
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents, minor: person.Minor + 1);
                        person.Minor += 1;
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "<-":
                    {
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents, minor: person.Minor - 1);
                        person.Minor -= 1;
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "Назад":
                    {
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
                        MessageConstructor.Back(person, e);
                        return;
                    }
                default:
                    {
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.RequestEvent, minor: (int)Create.Describe);
                        LookEvent(person, e);
                        return;
                    }
            }
        }

        internal static void LookEvent(Person person, MessageReceivedEventArgs e)
        {
            var @event = Event.GetEventFromActual(e.Message.Text);
            if (@event == null)
                MessageConstructor.WatchEventError(person, e);

            switch (e.Message.Text)
            {
                case "Назад":
                    {
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents, minor: (int)Create.Describe);
                        LookEvents(person, e);
                        return;
                    }
                case "Удалить":
                    {
                        EventsDatabase.EventSetParams(id: @event.Id, isActual: false);
                        Event.ActualEvents.Remove(@event);
                        MessageConstructor.WatchEvent(@event, person, e);
                        return;
                    }
                case "Редактировать": // работает как назад
                    {
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents, minor: (int)Create.Describe);
                        LookEvents(person, e);
                        return;
                    }
                default:
                    {
                        MessageConstructor.WatchEvent(@event, person, e);
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
                        //мероприятие готовое к созданию
                        var @event = EventsDatabase.FindEvents(id: Event.OnCreatedEvents.GetValueOrDefault(person.Id)).First();

                        //добавление мероприятьие в актуальные
                        Event.ActualEvents.Add(@event);

                        //делаем его актуальным
                        EventsDatabase.EventSetParams(@event.Id, isActual: true);

                        //удаление мероприятия из листа создаваемых мероприятий
                        Event.OnCreatedEvents.Remove(person.Id);

                        MessageConstructor.OnCreateEvent(person, e);
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
                        return;
                    }
                case "Название":
                    {
                        MessageConstructor.WaitingParameters_Name(person, e);
                        UsersDatabase.UserSetParams(person.Id, minor: (int)Create.Name);
                        return;
                    }
                case "Опиcание":
                    {
                        MessageConstructor.WaitingParameters_Describe(person, e);
                        UsersDatabase.UserSetParams(person.Id, minor: (int)Create.Describe);
                        return;
                    }
                case "Время начала":
                    {
                        MessageConstructor.WaitingParameters_StartTime(person, e);
                        UsersDatabase.UserSetParams(person.Id, minor: (int)Create.StartTime);
                        return;
                    }
                case "Время конца":
                    {
                        MessageConstructor.WaitingParameters_EndTime(person, e);
                        UsersDatabase.UserSetParams(person.Id, minor: (int)Create.EndTime);
                        return;
                    }
                case "Число волонтёров":
                    {
                        MessageConstructor.WaitingParameters_Seats(person, e);
                        UsersDatabase.UserSetParams(person.Id, minor: (int)Create.Seats);
                        return;
                    }
                case "Место":
                    {
                        MessageConstructor.WaitingParameters_Place(person, e);
                        UsersDatabase.UserSetParams(person.Id, minor: (int)Create.Place);
                        return;
                    }
                case "Назад":
                    {
                        MessageConstructor.Back(person, e);
                        UsersDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
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
            //получение создаваемого мероприятия
            var @event = EventsDatabase.FindEvents(id: Event.OnCreatedEvents.GetValueOrDefault(person.Id)).First();

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
                        if (e.Message.Text.AdvansedParse(out DateTime dateTime))
                            @event.StartTime = dateTime;
                        MessageConstructor.CreateEvent(person, e);
                        return;
                    }
                case Create.EndTime:
                    {
                        if (e.Message.Text.AdvansedParse(out DateTime dateTime))
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
