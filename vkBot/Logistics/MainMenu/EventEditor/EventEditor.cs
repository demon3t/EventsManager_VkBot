using System;
using EventsLogic.Basic;
using EventsLogic.HelperClasses;
using VkNet.Model.Keyboard;
using VkBotFramework.Models;
using vkBot.Logistics.MainMenu.ViewEvents.PickedEvent;
using static vkBot.Program;
using static vkBot.General.MessageGeneral;

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
            if (@event == null)
                @event = new Event("NaN");

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
            string message = client.Minor == (int)Major.CreateEvent ?
                "Создание нового мероприятия" :
                "Редактирование мероприятия";

            SendMessage(e, message, KeyboardThis(client));
        }

        #region Response options
        private static void Create(Client client, MessageReceivedEventArgs e, Event @event)
        {

        }

        #region Шаблоны для параметров
        //private static void Name(Client client)
        //{

        //}
        //private static void Description(Client client)
        //{

        //}
        //private static void StartTime(Client client)
        //{

        //}
        //private static void EndTime(Client client)
        //{

        //}
        //private static void Seats(Client client)
        //{

        //}
        //private static void Place(Client client)
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
                        return;
                    }
                case (int)Param.Describe:
                    {
                        @event.Describe = e.Message.Text;
                        return;
                    }
                case (int)Param.StartTime:
                    {
                        if (e.Message.Text.AdvansedParse(out DateTime dateTime))
                            @event.StartTime = dateTime;
                        return;
                    }
                case (int)Param.EndTime:
                    {
                        if (e.Message.Text.AdvansedParse(out DateTime dateTime))
                            @event.EndTime = dateTime;
                        return;
                    }
                case (int)Param.Seats:
                    {
                        if (int.TryParse(e.Message.Text, out int seats))
                            @event.Seats = seats;
                        return;
                    }
                case (int)Param.Place:
                    {
                        @event.Place = e.Message.Text;
                        return;
                    }
            }
        }

        private static void BackToMainMenu(Client client, MessageReceivedEventArgs e)
        {
            client.Major = (int)Major.Normal;
            client.Minor = 0;
            MainMenu.This(client, e);
        }

        private static void BackToPickedEvent(Client client, MessageReceivedEventArgs e, Event @event)
        {
            client.Major = (int)Major.PickedEvent;
            PickedEvent.This(client, e, @event);
        }

        #endregion

        private static KeyboardBuilder KeyboardThis(Client person)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();



            return keyboard;
        }
    }

}

