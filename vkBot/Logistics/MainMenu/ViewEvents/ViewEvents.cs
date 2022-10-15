using EventsLogic.Basic;
using System;
using VkBotFramework.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using static vkBot.General.MessageGeneral;
using static vkBot.Program;

namespace vkBot.Logistics.MainMenu.ViewEvents
{
    internal static class ViewEvents
    {
        #region propertis

        private static int _eventToList;

        static ViewEvents()
        {
            _eventToList = 5;
        }

        #endregion

        internal static void Go(Client client, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Посмотреть мероприятия": // ready
                    {
                        This(client, e); 
                        return;
                    }
                case "->": // ready
                    {
                        Next(client, e); 
                        return;
                    }
                case "<-": // ready
                    {
                        Prev(client, e); 
                        return;
                    }
                case "Назад": // ready
                    {
                        BackToMainMenu(client, e); 
                        return;
                    }
                default:
                    {
                        PickedEvent.PickedEvent.Go(client, e);
                        return;
                    }
            }
        }

        #region Response options

        internal static void This(Client client, MessageReceivedEventArgs e)
        {
            client.Major = (int)Major.ViewEvents;

            string message =
                $"Активные мероприятия {Environment.NewLine}" +
                $"Страница {client.Minor + 1}";

            SendMessage(e, message, KeyboardThis(client));
        }

        private static void Next(Client client, MessageReceivedEventArgs e)
        {
            client.Minor++;
            This(client, e);
        }

        private static void Prev(Client client, MessageReceivedEventArgs e)
        {
            client.Minor--;
            This(client, e);
        }

        private static void BackToMainMenu(Client client, MessageReceivedEventArgs e)
        {
            client.Major = (int)Major.Normal;
            client.Minor = 0;
            MainMenu.This(client, e);
        }

        #endregion

        private static KeyboardBuilder KeyboardThis(Client person)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            for (int i = person.Minor * _eventToList; i < person.Minor * _eventToList + _eventToList; i++)
            {
                if (i >= Event.ActualEvents.Count) break;
                keyboard.AddButton((i + 1).ToString(), "").AddLine();
            }

            if (person.Minor > 0)
            {
                keyboard
                    .AddButton("<-", "", KeyboardButtonColor.Primary);
                if (Event.ActualEvents.Count - person.Minor * _eventToList <= _eventToList)
                    keyboard
                        .AddLine();
            }

            if (Event.ActualEvents.Count - person.Minor * _eventToList > _eventToList)
                keyboard
                    .AddButton("->", "", KeyboardButtonColor.Primary).AddLine();

            keyboard
                .AddButton("Назад", "", KeyboardButtonColor.Primary);

            return keyboard;
        }
    }
}
