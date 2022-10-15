using EventsLogic.Basic;
using EventsLogic.DatabaseRequest;
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

     

        #endregion


        #region FirstOccurrence

        

        #endregion

        #region MainMenu

        public static void AboutMe(Client person, MessageReceivedEventArgs e)
        {
            string message =
                ClientDatabase.AboutMe(e.Message.PeerId.ToString());

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        public static void WatchEvents(Client person, MessageReceivedEventArgs e)
        {
            string message =
                $"Активые мероприятия{Environment.NewLine}" +
                $"{(Event.ActualEvents.Count > 5 ? $"{person.Minor + 1} страница" : "")}";

            SendMessage(e, message, KeyboardConstructor.WatchEvents(person, e));
        }

        #region WatchEvent
      

        public static void WatchEvent(Event @event, Client person, MessageReceivedEventArgs e)
        {
            string message =
                @event.ToString();

            SendMessage(e, message, KeyboardConstructor.OnEvent(@event, person, e));
        }

        #endregion


        #endregion

        #region Back

        public static void Back(Client person, MessageReceivedEventArgs e)
        {
            string message = "Возвращаю на главное меню";
            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

        #region CreateEvent

        /// <summary>
        /// Сообщение измменения параметра мероприятия
        /// </summary>
        public static void CreateEvent(Client person, MessageReceivedEventArgs e)
        {
            Event @event = EventsDatabase.FindEvents(id: Event.OnCreatedEvents.GetValueOrDefault(person.Id)).First();

            string message =
                @event.ToString();

            SendMessage(e, message, KeyboardConstructor.CreateEvents(e, @event));
        }


        /// <summary>
        /// Сообщение создания Мероприятия
        /// </summary>
        public static void OnCreateEvent(Client person, MessageReceivedEventArgs e)
        {
            Event @event = Event.ActualEvents.Find(x => x.Author == person.Id);

            string message =
                "Событие создано";

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

        #region WaitingParameters

        public static void WaitingParameters_Name(Client person, MessageReceivedEventArgs e)
        {
            string message =
                "Обязательное поля для заполнения";

            SendMessage(e, message);
        }

        public static void WaitingParameters_Describe(Client person, MessageReceivedEventArgs e)
        {
            string message =
                "Не обязательное поля для заполнения";

            SendMessage(e, message);
        }

        public static void WaitingParameters_StartTime(Client person, MessageReceivedEventArgs e)
        {
            string message =
                $"Обязательное поля для заполнения {Environment.NewLine}" +
                $"Формат записи: дд.мм.гггг чч:мм";

            SendMessage(e, message);
        }

        public static void WaitingParameters_EndTime(Client person, MessageReceivedEventArgs e)
        {
            string message =
                $"Обязательное поля для заполнения {Environment.NewLine}" +
                $"Формат записи: дд.мм.гггг чч:мм";

            SendMessage(e, message);
        }

        public static void WaitingParameters_Seats(Client person, MessageReceivedEventArgs e)
        {
            string message =
                "Обязательное поля для заполнения";

            SendMessage(e, message);
        }

        public static void WaitingParameters_Place(Client person, MessageReceivedEventArgs e)
        {
            string message =
                "Не обязательное поля для заполнения";

            SendMessage(e, message, KeyboardConstructor.WaitingParameters_Place());
        }

        #endregion

    }
}

