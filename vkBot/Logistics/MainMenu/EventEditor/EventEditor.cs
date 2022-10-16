using System;
using EventsLogic.Basic;
using EventsLogic.HelperClasses;
using VkNet.Model.Keyboard;
using VkBotFramework.Models;
using vkBot.Logistics.MainMenu.ViewEvents.PickedEvent;
using vkBot.HelperElements;
using vkBot.Request;
using static vkBot.Program;
using static vkBot.General.KeyboardGeneral;
using static vkBot.General.MessageGeneral;
using static vkBot.Request.EventRequest;

namespace vkBot.Logistics.MainMenu.EventEditor
{
    internal static class EventEditor
    {
        private enum Param
        {
            Name = 1,
            Describe = 2,
            StartTime = 3,
            EndTime = 4,
            Seats = 5,
            Place = 6,
        }

        internal static void Go(Client client, MessageReceivedEventArgs e, Event @event = null)
        {
            if (Event.OnCreatedEvents.TryGetValue(client.Id, out int eventId))
            {
                @event = Get(eventId);
            }
            else
            {
                Add(out @event, client.Id);
            }

            switch (e.Message.Text)
            {
                case "Создать мероприятие":
                    {
                        This(client, e, @event);
                        return;
                    }
                case "Редактировать":
                    {
                        This(client, e, @event);
                        return;
                    }
                case "Создать":
                    {
                        Create(client, e, @event);
                        return;
                    }
                case "Название":
                    {
                        client.TempMinor = (int)Param.Name;
                        return;
                    }
                case "Опиcание":
                    {
                        client.TempMinor = (int)Param.Describe;
                        return;
                    }
                case "Время начала":
                    {
                        client.TempMinor = (int)Param.StartTime;
                        return;
                    }
                case "Время конца":
                    {
                        client.TempMinor = (int)Param.EndTime;
                        return;
                    }
                case "Число волонтёров":
                    {
                        client.TempMinor = (int)Param.Seats;
                        return;
                    }
                case "Место":
                    {
                        client.TempMinor = (int)Param.Place;
                        return;
                    }
                case "Назад":
                    {
                        BackToMainMenu(client, e);
                        return;
                    }
                case "Вернуться":
                    {
                        BackToPickedEvent(client, e, @event);
                        return;
                    }
                default:
                    {
                        SetParam(client, e, @event);
                        return;
                    }
            }
        }

        internal static void This(Client client, MessageReceivedEventArgs e, Event @event)
        {
            string message = client.Major == (int)Major.Normal ?
                "Создание нового мероприятия" :
                "Редактирование мероприятия";

            client.Major = client.Major == (int)Major.Normal ?
                (int)Major.CreateEvent :
                (int)Major.EdingEvent;

            if (!Event.OnCreatedEvents.ContainsKey(client.Id))
                Event.OnCreatedEvents.Add(client.Id, @event.Id);

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        #region Response options
        private static void Create(Client client, MessageReceivedEventArgs e, Event @event)
        {
            Event.OnCreatedEvents.Remove(client.Id);
            BackToMainMenu(client, e);
        }

        #region SetParam
        //private static void Name(Client client, MessageReceivedEventArgs e, Event @event)
        //{

        //}
        //private static void Description(Client client, MessageReceivedEventArgs e, Event @event)
        //{

        //}
        //private static void StartTime(Client client, MessageReceivedEventArgs e, Event @event)
        //{

        //}
        //private static void EndTime(Client client, MessageReceivedEventArgs e, Event @event)
        //{

        //}
        //private static void Seats(Client client, MessageReceivedEventArgs e, Event @event)
        //{

        //}
        //private static void Place(Client client, MessageReceivedEventArgs e, Event @event)
        //{

        //}
        #endregion

        private static void SetParam(Client client, MessageReceivedEventArgs e, Event @event)
        {
            switch (client.TempMinor)
            {
                case (int)Param.Name:
                    {
                        @event.Name = e.Message.Text;
                        break;
                    }
                case (int)Param.Describe:
                    {
                        @event.Describe = e.Message.Text;
                        break;
                    }
                case (int)Param.StartTime:
                    {
                        if (e.Message.Text.AdvansedParse(out DateTime dateTime))
                            @event.StartTime = dateTime;
                        break;
                    }
                case (int)Param.EndTime:
                    {
                        if (e.Message.Text.AdvansedParse(out DateTime dateTime))
                            @event.EndTime = dateTime;
                        break;
                    }
                case (int)Param.Seats:
                    {
                        if (int.TryParse(e.Message.Text, out int seats))
                            @event.Seats = seats;
                        break;
                    }
                case (int)Param.Place:
                    {
                        @event.Place = e.Message.Text;
                        break;
                    }
            }
            Continue(client, e, @event);
        }

        private static void Continue(Client client, MessageReceivedEventArgs e, Event @event)
        {
            string message = @event.Ready() ?
                "Всё корректно" :
                "Не всё корректно";

            SendMessage(e, message, KeyboardThis(client, @event));
        }

        private static void BackToMainMenu(Client client, MessageReceivedEventArgs e)
        {
            if (client.Major != (int)Major.CreateEvent) return;
            client.Major = (int)Major.Normal;
            client.Minor = 0;
            MainMenu.This(client, e);
        }

        private static void BackToPickedEvent(Client client, MessageReceivedEventArgs e, Event @event)
        {
            if (client.Major != (int)Major.EdingEvent) return;
            Event.OnCreatedEvents.Remove(client.Id);
            client.Major = (int)Major.PickedEvent;
            PickedEvent.This(client, e, @event);
        }

        #endregion

        private static KeyboardBuilder KeyboardThis(Client client, Event @event)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            if (client.Major == (int)Major.CreateEvent && @event.Ready()) keyboard.AddButton("Создать", "", Primary).AddLine();

            keyboard.AddButton("Название", "", @event.Name.IsNaN() ? Negative : Positive);
            keyboard.AddButton("Опиcание", "", @event.Name.IsNaN() ? Default : Positive).AddLine();
            keyboard.AddButton("Время начала", "", @event.StartTime < DateTime.Now ? Negative : Positive);
            keyboard.AddButton("Время конца", "", @event.EndTime < DateTime.Now || @event.EndTime < @event.StartTime ? Negative : Positive).AddLine();
            keyboard.AddButton("Число волонтёров", "", @event.Seats < 1 ? Negative : Positive);
            keyboard.AddButton("Место", "", @event.Name.IsNaN() ? Default : Positive).AddLine();

            if (client.Major == (int)Major.CreateEvent)
                keyboard.AddButton("Назад", "", Default);
            else
                keyboard.AddButton("Вернуться", "", Default);

            return keyboard;
        }
    }

}

