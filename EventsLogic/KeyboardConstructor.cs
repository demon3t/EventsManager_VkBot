using System;
using System.Data;
using VkBotFramework.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace EventsLogic
{
    internal class KeyboardConstructor
    {
        static KeyboardButtonColor Primary = KeyboardButtonColor.Primary;
        static KeyboardButtonColor Positive = KeyboardButtonColor.Positive;
        static KeyboardButtonColor Negative = KeyboardButtonColor.Negative;
        static KeyboardButtonColor Default = KeyboardButtonColor.Default;


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

        internal static KeyboardBuilder WatchEvents(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            foreach (var _event in Event.ActualEvents)
            {
                if (_event.Name == null) continue;
                keyboard.AddButton(_event.Name.ToString(), "").AddLine();
            }

            keyboard
                .AddButton("Назад", "", KeyboardButtonColor.Primary);
            return keyboard;
        }

        internal static KeyboardBuilder MyEvents(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            keyboard
                .AddButton("Назад", "", KeyboardButtonColor.Primary)
                .SetOneTime();
            return keyboard;
        }

        /// <summary>
        /// Клавиатура при создании мероприятия
        /// </summary>
        internal static KeyboardBuilder CreateEvents(MessageReceivedEventArgs e, Event @event)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

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

                .AddButton("Назад","Кнопка возвращает на главное меню" , Default)
                .SetOneTime();
            return keyboard;
        }

        internal static KeyboardBuilder CompletEvents(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            keyboard
                .AddButton("Назад", "", Default)
                .SetOneTime();
            return keyboard;
        }

        #endregion
    }
}
