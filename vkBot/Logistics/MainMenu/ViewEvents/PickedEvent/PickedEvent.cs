using System;
using EventsLogic.Basic;
using VkNet.Model.Keyboard;
using VkBotFramework.Models;
using VkNet.Enums.SafetyEnums;
using static vkBot.Program;
using static vkBot.General.MessageGeneral;
using static vkBot.General.KeyboardGeneral;

namespace vkBot.Logistics.MainMenu.ViewEvents.PickedEvent
{
    internal static class PickedEvent
    {
        internal static void Go(Client client, MessageReceivedEventArgs e)
        {
            var @event = new Event(String.Empty);

            switch (e.Message.Text)
            {
                case "Я пойду": // ready
                    {
                        Come(client, e, @event);
                        return;
                    }
                case "Я не пойду": // ready
                    {
                        NotCome(client, e, @event);
                        return;
                    }
                case "Редактировать": // coomin soon -
                    {
                        Editing(client, e, @event);
                        return;
                    }
                case "Удалить": // coomin soon -
                    {
                        Delete(client, e, @event);
                        return;
                    }
                case "Назад": // ready
                    {
                        BackToViewEvents(client, e, @event);
                        return;
                    }
                default:
                    {
                        This(client, e, @event);
                        return;
                    }
            }
        }

        #region Response options

        internal static void This(Client client, MessageReceivedEventArgs e, Event @event)
        {
            client.Major = (int)Major.PickedEvent;

            string message =
                $"Информация о мероприятии";

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        private static void Come(Client client, MessageReceivedEventArgs e, Event @event)
        {
            string message =
                $"Не идёшь";

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        private static void NotCome(Client client, MessageReceivedEventArgs e, Event @event)
        {
            string message =
                $"Идешь";

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        private static void Editing(Client client, MessageReceivedEventArgs e, Event @event)
        {
            if (!client.IsAdmin) return;

            string message =
                $"Редактирование";

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        private static void Delete(Client client, MessageReceivedEventArgs e, Event @event)
        {
            if (!client.IsAdmin) return;

            string message =
                $"Удаление";

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        private static void BackToViewEvents(Client client, MessageReceivedEventArgs e, Event @event)
        {
            client.Major = (int)Major.ViewEvents;
            ViewEvents.This(client, e);
        }

        #endregion

        private static KeyboardBuilder KeyboardThis(Client person, Event @event)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();


            if (true)
            {
                keyboard.AddButton("Я пойду", "", Positive).AddLine();
            }
            else
            {
                if (DateTime.Now.AddHours(1) < @event.StartTime)
                {
                    keyboard.AddButton("Я не пойду", "", Negative).AddLine();
                }

            }

            if (person.IsAdmin)
            {
                keyboard
                    .AddButton("Редактироварь", "", Primary).AddLine()
                    .AddButton("Удалить", "", Negative).AddLine();
            }

            keyboard.AddButton("Назад", "", KeyboardButtonColor.Primary);

            return keyboard;
        }
    }
}
