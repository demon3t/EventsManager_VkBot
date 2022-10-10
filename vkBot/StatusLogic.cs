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
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.First);
                    }
                    return;
                case "Опивание":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Second);
                    }
                    return;
                case "Дата":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Third);
                    }
                    return;
                case "Время":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Fourth);
                    }
                    return;
                case "Число волонтёров":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Five);
                    }
                    return;
                case "Место":
                    {
                        MessageConstructor.WatchEvents_Back(person, e);
                        DatebaseLogic.UserSetParams(person, minor: (int)Minor.Sixth);
                    }
                    return;
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
    }
}
