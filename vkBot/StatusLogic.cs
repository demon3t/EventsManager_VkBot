using EventsLogic;
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

        private enum Minor
        {
            First = 0,
            Second = 1,
            Third = 2,
            Fourth = 3,
            Five = 4,
            Sixth = 5,
        }



        internal static void FirstOccurrence(Person person, MessageReceivedEventArgs e)
        {
            MessageConstructor.FirstOccurrence(person, e);
            DatebaseLogic.UserSetParams(person, major: (int)Major.Normal, minor: (int)Minor.First);
        }


        internal static void MainMenu(Person person, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Посмотреть мероприятия":
                    {
                        MessageConstructor.WatchEvents(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.LookEvents, minor: (int)Minor.First);
                    }
                    return;
                case "Создать мероприятие":
                    {
                        if (!person.IsAdmin) return;
                        Event.CreateEvent(person.Id);
                        MessageConstructor.CreateEvents(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.CreateEvent, minor: (int)Minor.First);
                    }
                    return;
                case "Мои мероприятия":
                    {
                        MessageConstructor.MyEvents(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.MyEvents, minor: (int)Minor.First);
                    }
                    return;
                case "Завершённые мероприятия":
                    {
                        MessageConstructor.CompletEvents(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.CompletEvents, minor: (int)Minor.First);
                    }
                    return;
                case "Информация":
                    {
                        FirstOccurrence(person, e);
                    }
                    return;
                case "Обо мне":
                    {
                        MessageConstructor.AboutMe(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.Normal, minor: (int)Minor.First);
                    }
                    return;
                default: return;
            }
        }

        internal static void WatchingEvents(Person person, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Назад":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.Normal, minor: (int)Minor.First);
                    }
                    return;
                default:
                    {

                    }
                    return;
            }
        }

        internal static void CreateEvent(Person person, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Название":
                    {
                        MessageConstructor.WaitingParameters_Name(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.First);
                    }
                    return;
                case "Опиcание":
                    {
                        MessageConstructor.WaitingParameters_Describe(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Second);
                    }
                    return;
                case "Дата/Время начала":
                    {
                        MessageConstructor.WaitingParameters_StartTime(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Third);
                    }
                    return;
                case "Дата/Время конца":
                    {
                        MessageConstructor.WaitingParameters_EndTime(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Fourth);
                    }
                    return;
                case "Число волонтёров":
                    {
                        MessageConstructor.WaitingParameters_Seats(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Five);
                    }
                    return;
                case "Место":
                    {
                        MessageConstructor.WaitingParameters_Place(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Sixth);
                    }
                    return;
                case "Назад":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, major: (int)Major.Normal, minor: (int)Minor.First);
                        Event.RemoveEvent(person.Id);
                    }
                    return;
                default:
                    {
                        Event @event = Event.ActualEvents.Find(x => x.PersonCreated == person.Id);

                        switch ((Minor)person.Minor)
                        {
                            case Minor.First:
                                {
                                    @event.Name = e.Message.Text;
                                    MessageConstructor.CreateEvent(person, e);
                                }
                                return;
                            case Minor.Second:
                                {
                                    @event.Describe = e.Message.Text;
                                    MessageConstructor.CreateEvent(person, e);
                                }
                                return;
                            case Minor.Third:
                                {
                                    if (DateTime.TryParse(e.Message.Text, out DateTime dateTime))
                                        @event.StartTime = dateTime;
                                    MessageConstructor.CreateEvent(person, e);
                                }
                                return;
                            case Minor.Fourth:
                                {
                                    if (DateTime.TryParse(e.Message.Text, out DateTime dateTime))
                                        @event.EndTime = dateTime;
                                    MessageConstructor.CreateEvent(person, e);
                                }
                                return;
                            case Minor.Five:
                                {
                                    if (int.TryParse(e.Message.Text, out int seats))
                                        @event.Seats = seats;
                                    MessageConstructor.CreateEvent(person, e);
                                }
                                return;
                            case Minor.Sixth:
                                {
                                    @event.Place = e.Message.Text;
                                    MessageConstructor.CreateEvent(person, e);
                                }
                                return;
                        }
                    }
                    return;
            }
        }
    }
}
