using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace EventsLogic
{
    static public class MessageConstructor
    {
        public static VkBot? vkBot { private get; set; }
        public static string connectionDbString { private get; set; } = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security=True";

        private static void SendMessage(MessageReceivedEventArgs e, string message, KeyboardBuilder keyboard)
        {
            if (vkBot == null) return;

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build()
            });
        }

        #region FirstOccurrence

        public static void FirstOccurrence(Person person, MessageReceivedEventArgs e)
        {
            string message =
                $"Привет {person.Name}. Данный бот может оповещать и записывать на доступные мероприятия.\n" +
                $"А так же напоминать о мероприятиях на которые Вы записались.";

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

        #region MainMenu

        public static void AboutMe(Person person, MessageReceivedEventArgs e)
        {
            string message =
                DatebaseLogic.AboutMe(e.Message.PeerId.ToString());

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        public static void WatchEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Все активные мероприятия";

            SendMessage(e, message, KeyboardConstructor.WatchEvents(e));
        }

        public static void MyEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Ваши мероприятия";

            SendMessage(e, message, KeyboardConstructor.MyEvents(e));
        }

        public static void CreateEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Необходимо заполнить всю обязательную информация (кнопки красного цвета).";

            SendMessage(e, message, KeyboardConstructor.CreateEvents(e));
        }

        public static void CompletEvents(Person person, MessageReceivedEventArgs e)
        {
            string message =
                "Необходимо заполнить всю обязательную информация (кнопки красного цвета).";

            SendMessage(e, message, KeyboardConstructor.CompletEvents(e));
        }


        #endregion

        #region WatchingEvents

        public static void WatchEvents_Back(Person person, MessageReceivedEventArgs e)
        {
            string message = "Возвращаю на главное меню";

            SendMessage(e, message, KeyboardConstructor.MainMenu(person.IsAdmin));
        }

        #endregion

    }
}

