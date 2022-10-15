using EventsLogic.Basic;
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

        internal static KeyboardBuilder WatchEvents(Client person, MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            for (int i = person.Minor * EventToList; i < person.Minor * EventToList + EventToList; i++)
            {
                if (i >= Event.ActualEvents.Count) break;
                keyboard.AddButton((i + 1).ToString(), "").AddLine();
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

        internal static KeyboardBuilder OnEvent(Event @event, Client person, MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();


            if (true)
                keyboard.AddButton("Я пойду", "", Positive).AddLine();
            else
                if (DateTime.Now.AddHours(1) < @event.StartTime)
                keyboard.AddButton("Я не пойду", "", Negative).AddLine();

            if (person.IsAdmin)
                keyboard
                    .AddButton("Удалить", "", Negative).AddLine()
                    .AddButton("Редактироварь", "", Primary).AddLine();

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
