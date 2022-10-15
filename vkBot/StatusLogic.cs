using EventsLogic;
using EventsLogic.DatabaseRequest;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using VkBotFramework.Models;
using EventsLogic.HelperClasses;
using System.Linq;
using EventsLogic.Basic;
using vkBot.Logistics;

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




        #region Посмотреть мероприятия


        internal static void LookEvent(Client person, MessageReceivedEventArgs e)
        {
            var @event = Event.GetEventFromActual(e.Message.Text);
            if (@event == null)
                @event = Event.ActualEvents[person.Minor];
            else
                ClientDatabase.UserSetParams(person.Id, minor: Event.ActualEvents.IndexOf(@event));

            switch (e.Message.Text)
            {
                case "Назад":
                    {
                        ClientDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents);
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "Удалить":
                    {
                        EventsDatabase.EventSetParams(id: @event.Id, isActual: false);
                        Event.ActualEvents.Remove(@event);
                        ClientDatabase.UserSetParams(person.Id, minor: 0);
                        MessageConstructor.WatchEvents(person, e);
                        return;
                    }
                case "Редактировать": // работает как назад
                    {
                        ClientDatabase.UserSetParams(person.Id, major: (int)Major.LookEvents);
                        MessageConstructor.WatchEvents(person, e);
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

        internal static void CreateEvent(Client person, MessageReceivedEventArgs e)
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
                        ClientDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
                        return;
                    }
                case "Название":
                    {
                        MessageConstructor.WaitingParameters_Name(person, e);
                        ClientDatabase.UserSetParams(person.Id, minor: (int)Create.Name);
                        return;
                    }
                case "Опиcание":
                    {
                        MessageConstructor.WaitingParameters_Describe(person, e);
                        ClientDatabase.UserSetParams(person.Id, minor: (int)Create.Describe);
                        return;
                    }
                case "Время начала":
                    {
                        MessageConstructor.WaitingParameters_StartTime(person, e);
                        ClientDatabase.UserSetParams(person.Id, minor: (int)Create.StartTime);
                        return;
                    }
                case "Время конца":
                    {
                        MessageConstructor.WaitingParameters_EndTime(person, e);
                        ClientDatabase.UserSetParams(person.Id, minor: (int)Create.EndTime);
                        return;
                    }
                case "Число волонтёров":
                    {
                        MessageConstructor.WaitingParameters_Seats(person, e);
                        ClientDatabase.UserSetParams(person.Id, minor: (int)Create.Seats);
                        return;
                    }
                case "Место":
                    {
                        MessageConstructor.WaitingParameters_Place(person, e);
                        ClientDatabase.UserSetParams(person.Id, minor: (int)Create.Place);
                        return;
                    }
                case "Назад":
                    {
                        MessageConstructor.Back(person, e);
                        ClientDatabase.UserSetParams(person.Id, major: (int)Major.Normal, minor: 0);
                        return;
                    }
                default:
                    {
                        SetParamEvent(person, e);
                        return;
                    }
            }
        }
        internal static void SetParamEvent(Client person, MessageReceivedEventArgs e)
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
