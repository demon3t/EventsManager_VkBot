using EventsLogic.DatabaseRequest;
using System;
using System.Data;
using VkBotFramework.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace EventsLogic
{
    internal static class KeyboardConstructor
    {
        static KeyboardButtonColor Primary = KeyboardButtonColor.Primary;
        static KeyboardButtonColor Positive = KeyboardButtonColor.Positive;
        static KeyboardButtonColor Negative = KeyboardButtonColor.Negative;
        static KeyboardButtonColor Default = KeyboardButtonColor.Default;


        private const int EventToList = 5;

        #region MainMenu

        internal static KeyboardBuilder MainMenu(bool IsAdmin)
        {
            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
            .AddButton("Посмотреть мероприятия", "", Positive)
            .AddLine();

            if (IsAdmin)
                keyboard
                    .AddButton("Создать мероприятие", "", Primary)
                    .AddLine();

            keyboard
                .AddButton("Мои мероприятия", "", Primary)
            .AddButton("Завершённые мероприятия", "", KeyboardButtonColor.Default).SetOneTime()
            .AddLine()
            .AddButton("Информация", "", Default)
            .AddButton("Обо мне", "", Default);

            return keyboard;
        }

        internal static KeyboardBuilder WatchEvents(Person person, MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

                for (int i = person.Minor * EventToList; i < person.Minor * EventToList + EventToList; i++)
                {
                    if (i >= Event.ActualEvents.Count) break;
                    if (Event.ActualEvents[i].Name != null)
                        keyboard.AddButton(Event.ActualEvents[i].Name, "").AddLine();
                }

                if (person.Minor > 0)
                {
                    keyboard
                        .AddButton("<-", "", KeyboardButtonColor.Primary);
                    if (Event.ActualEvents.Count - person.Minor * EventToList <= EventToList)
                        keyboard
                            .AddLine();
                }

                if (Event.ActualEvents.Count - person.Minor * EventToList > EventToList)
                    keyboard
                        .AddButton("->", "", KeyboardButtonColor.Primary).AddLine();
            
            keyboard
                .AddButton("Назад", "", KeyboardButtonColor.Primary);
            return keyboard;
        }

        internal static KeyboardBuilder MyEvents(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            keyboard
                .AddButton("Назад", "", KeyboardButtonColor.Primary);
            return keyboard;
        }

        /// <summary>
        /// Клавиатура при создании мероприятия
        /// </summary>
        internal static KeyboardBuilder CreateEvents(MessageReceivedEventArgs e, Event @event)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder("type", false);

            if (@event.CheackReady())
                keyboard
                .AddButton("Создать", "", Primary).AddLine();

            keyboard
                .AddButton("Название", "",
                string.IsNullOrWhiteSpace(@event.Name) ? Negative : Positive)

                .AddButton("Опиcание", "",
                string.IsNullOrWhiteSpace(@event.Describe) ? Default : Positive).AddLine()

                .AddButton("Время начала", "",
                @event.StartTime < DateTime.Now ? Negative : Positive)

                .AddButton("Время конца", "",
                (@event.EndTime < @event.StartTime && @event.EndTime < DateTime.Now) ? Negative : Positive).AddLine()

                .AddButton("Число волонтёров", "",
                @event.Seats < 1 ? Negative : Positive)

                .AddButton("Место", "",
                string.IsNullOrWhiteSpace(@event.Place) ? Default : Positive).AddLine()

                .AddButton("Назад", "Кнопка возвращает на главное меню", Default);
            return keyboard;
        }

        internal static KeyboardBuilder CompletEvents(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            keyboard
                .AddButton("Назад", "", Default);
            return keyboard;

        }

        #endregion

        #region WaitingParameters

        //public static void WaitingParameters_Name(Person person, MessageReceivedEventArgs e)
        //{
        //    string message = "Обязательное поля для заполнения";

        //    SendMessage(e, message);
        //}

        //public static void WaitingParameters_Describe(Person person, MessageReceivedEventArgs e)
        //{
        //    string message = "Не обязательное поля для заполнения";

        //    SendMessage(e, message);
        //}

        //public static void WaitingParameters_StartTime(Person person, MessageReceivedEventArgs e)
        //{
        //    string message =
        //        "Обязательное поля для заполнения\n" +
        //        "Формат записи: дд.мм.гггг чч:мм";

        //    SendMessage(e, message);
        //}

        //public static void WaitingParameters_EndTime(Person person, MessageReceivedEventArgs e)
        //{
        //    string message =
        //        "Обязательное поля для заполнения\n" +
        //        "Формат записи: дд.мм.гггг чч:мм";

        //    SendMessage(e, message);
        //}

        //public static void WaitingParameters_Seats(Person person, MessageReceivedEventArgs e)
        //{
        //    string message = "Обязательное поля для заполнения";

        //    SendMessage(e, message);
        //}

        public static KeyboardBuilder WaitingParameters_Place()
        {
            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder();

            keyboard.AddGeo();

            return keyboard;
        }

        #endregion

        #region SpetionButton

        private static void AddGeo(this KeyboardBuilder keyboard)
        {
            keyboard.AddButton("Геопозиция", "location", Default);
        }

        #endregion
    }
}
