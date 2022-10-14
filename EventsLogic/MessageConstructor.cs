﻿using EventsLogic.DatabaseRequest;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace EventsLogic
{
    static public class MessageConstructor
    {
        public static VkBot? vkBot { private get; set; }

        private static void SendMessage(MessageReceivedEventArgs e, string message, KeyboardBuilder keyboard)
        {
            if (vkBot == null) return;

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                UserId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build(),
            });
        }
        private static void SendMessage(MessageReceivedEventArgs e, string message)
        {
            if (vkBot == null) return;

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                UserId = e.Message.PeerId,
                RandomId = Environment.TickCount,
            });
        }

        #region RequestEvent

        public static void RequestEvent(Person person, MessageReceivedEventArgs e)
        {

            //string message =


            //SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion


        #region FirstOccurrence

        public static void FirstOccurrence(Person person, MessageReceivedEventArgs e)
        {
            string message =
                $"Привет {person.Name}. Данный бот может:{Environment.NewLine}" +
                $"➤  Записывать на доступные мероприятия;{Environment.NewLine}" +
                $"➤  Заранее оповещать о начале мероприятий;{Environment.NewLine}" +
                $"➤  Показать посещённые мероприяти;{Environment.NewLine}" +
                $"➤  Показывать статистику.";

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

        #region MainMenu

        public static void AboutMe(Person person, MessageReceivedEventArgs e)
        {
            string message =
                UsersDatabase.AboutMe(e.Message.PeerId.ToString());

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        public static void WatchEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                $"Активые мероприятия{Environment.NewLine}" +
                $"{(Event.ActualEvents.Count > 5 ? $"{person.Minor + 1} страница" : "")}";

            SendMessage(e, message, KeyboardConstructor.WatchEvents(person, e));
        }

        #region WatchEvent
        public static void WatchEventError(Person person, MessageReceivedEventArgs e)
        {
            string message =
                    "Что-то пошло не так";

            SendMessage(e, message, KeyboardConstructor.WatchEvents(person, e));
        }

        public static void WatchEvent(Event @event, Person person, MessageReceivedEventArgs e)
        {
            string message =
                @event.ToString();

            SendMessage(e, message, KeyboardConstructor.OnEvent(@event, person, e));
        }

        #endregion

        public static void MyEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Ваши мероприятия";

            SendMessage(e, message, KeyboardConstructor.MyEvents(e));
        }


        public static void CompletEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Необходимо заполнить всю обязательную информация (кнопки красного цвета).";

            SendMessage(e, message, KeyboardConstructor.CompletEvents(e));
        }


        #endregion

        #region Back

        public static void Back(Person person, MessageReceivedEventArgs e)
        {
            string message = "Возвращаю на главное меню";
            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

        #region CreateEvent

        /// <summary>
        /// Сообщение измменения параметра мероприятия
        /// </summary>
        public static void CreateEvent(Person person, MessageReceivedEventArgs e)
        {
            Event @event = EventsDatabase.FindEvents(id: Event.OnCreatedEvents.GetValueOrDefault(person.Id)).First();

            string message =
                @event.ToString();

            SendMessage(e, message, KeyboardConstructor.CreateEvents(e, @event));
        }


        /// <summary>
        /// Сообщение создания Мероприятия
        /// </summary>
        public static void OnCreateEvent(Person person, MessageReceivedEventArgs e)
        {
            Event @event = Event.ActualEvents.Find(x => x.Author == person.Id);

            string message =
                "Событие создано";

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

        #region WaitingParameters

        public static void WaitingParameters_Name(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Обязательное поля для заполнения";

            SendMessage(e, message);
        }

        public static void WaitingParameters_Describe(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Не обязательное поля для заполнения";

            SendMessage(e, message);
        }

        public static void WaitingParameters_StartTime(Person person, MessageReceivedEventArgs e)
        {
            string message =
                $"Обязательное поля для заполнения {Environment.NewLine}" +
                $"Формат записи: дд.мм.гггг чч:мм";

            SendMessage(e, message);
        }

        public static void WaitingParameters_EndTime(Person person, MessageReceivedEventArgs e)
        {
            string message =
                $"Обязательное поля для заполнения {Environment.NewLine}" +
                $"Формат записи: дд.мм.гггг чч:мм";

            SendMessage(e, message);
        }

        public static void WaitingParameters_Seats(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Обязательное поля для заполнения";

            SendMessage(e, message);
        }

        public static void WaitingParameters_Place(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Не обязательное поля для заполнения";

            SendMessage(e, message, KeyboardConstructor.WaitingParameters_Place());
        }

        #endregion

        public static void Error(Person person, MessageReceivedEventArgs e)
        {
            string message = "Error massage";
            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }
    }
}

