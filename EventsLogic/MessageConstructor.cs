﻿using Microsoft.IdentityModel.Tokens;
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


        public static void ButtonBegin(MessageReceivedEventArgs e)
        {
            Event.AllInterestUsers.Remove(e.Message.PeerId.ToString());

            string message =
                "Привет, уже был здесь?";

            SendMessage(e, message, KeyboardConstructor.KeyboardBegin());
        }

        public static void ButtonInfo(MessageReceivedEventArgs e)
        {
            Event.AllInterestUsers.Remove(e.Message.PeerId.ToString());

            string message =
                $"Привет {1}.Данный бот может оповещать и записывать на доступные мероприятия.\n" +
                $"А так же напоминать о мероприятиях на которые Вы записались.";

            SendMessage(e, message, KeyboardConstructor.KeyboardInfo());
        }

        public static void ButtonAboutMe(MessageReceivedEventArgs e, bool IsAdmin)
        {
            Event.AllInterestUsers.Remove(e.Message.PeerId.ToString());

            string message =
                DatebaseLogic.AboutMe(e.Message.PeerId.ToString());

            SendMessage(e, message, KeyboardConstructor.MainMenu(IsAdmin));
        }

        public static void ButtonKnown(MessageReceivedEventArgs e, bool IsAdmin)
        {
            Event.AllInterestUsers.Remove(e.Message.PeerId.ToString());

            string message =
                "Тогда продолжим.\n" +
                "Что ты хочешь сделать?";

            SendMessage(e, message, KeyboardConstructor.MainMenu(IsAdmin));
        }

        public static void ButtonLookEvents(MessageReceivedEventArgs e)
        {
            string message =
                "Все активные мероприятия";

            SendMessage(e, message, KeyboardConstructor.KeyboardLookEvent(e));
        }

        public static void BullonEvent(MessageReceivedEventArgs e, bool IsAdmin, Event selectEvent)
        {
            Event.AllInterestUsers.Remove(e.Message.PeerId.ToString());
            Event.AllChoiseUsers.Add(e.Message.PeerId.ToString());
            selectEvent.ChoiseUsers.Add(e.Message.PeerId.ToString());

            string message =
                 $"{selectEvent.Name}\n" +
                 $"Время: {selectEvent.FullDataTime:f}\n" +
                 $"{(selectEvent.Place == null ? "" : $"Место проведения: {selectEvent.Place}\n")}" +
                 $"{(selectEvent.Describe == null ? "" : $"О мероприятии: {selectEvent.Describe}\n")}" +
                 $"Свободных мест: {selectEvent._Count} из {selectEvent.Count}";

            SendMessage(e, message, KeyboardConstructor.KeyboardEvent(e, selectEvent, IsAdmin));

        }

        #region choise

        public static void ButtonGo(MessageReceivedEventArgs e, bool IsAdmin)
        {
            Event @event = new Event();

            foreach (var _event in Event.ActualEvents)
            {
                foreach (var user in _event.ChoiseUsers)
                    if (user == e.Message.PeerId.ToString())
                    {
                        @event = _event;
                        @event._Count++;
                        @event.InvolvedUsers.Add(user);
                        Event.AllChoiseUsers.Remove(user);
                        Event.AllInterestUsers.Remove(user);
                        break;
                    }
                if (!string.IsNullOrEmpty(@event.Name)) break;
            }

            string message =
                $"Вы записалисы на мероприятие\n" +
                $"\"{@event.Name}\"";

            SendMessage(e, message, KeyboardConstructor.MainMenu(IsAdmin));
        }

        public static void ButtonNotGo(MessageReceivedEventArgs e, bool IsAdmin)
        {
            Event @event = new Event();

            foreach (var _event in Event.ActualEvents)
            {
                foreach (var user in _event.InvolvedUsers)
                    if (user == e.Message.PeerId.ToString())
                    {
                        @event = _event;
                        @event._Count--;
                        @event.InvolvedUsers.Remove(user);
                        Event.AllChoiseUsers.Remove(user);
                        break;
                    }
                if (!string.IsNullOrEmpty(@event.Name)) break;
            }


            string message =
                $"Вы не пойдёте на мероприятие:\n" +
                $"\"{@event.Name}\"";

            SendMessage(e, message, KeyboardConstructor.MainMenu(IsAdmin));
        }

        public static void ButtunExitChoise(MessageReceivedEventArgs e, bool IsAdmin)
        {
            Event.AllInterestUsers.Remove(e.Message.PeerId.ToString());
            Event.AllChoiseUsers.Remove(e.Message.PeerId.ToString());

            string message =
                "Что ты хотите сделать?";

            SendMessage(e, message, KeyboardConstructor.MainMenu(IsAdmin));
        }
        #endregion
    }
}
