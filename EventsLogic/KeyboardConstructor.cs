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

            foreach (var _event in actualEveints)
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

        internal static KeyboardBuilder CreateEvents(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            keyboard
                .AddButton("Название", "", Negative).AddLine()
                .AddButton("Опивание", "", Negative).AddLine()
                .AddButton("Дата", "", Negative).AddLine()
                .AddButton("Время", "", Negative).AddLine()
                .AddButton("Число волонтёров", "", Negative).AddLine()
                .AddButton("Место", "", Default).AddLine()
                .AddButton("Назад", "", Default);
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

        #region WatchingEvents

        #endregion


    }
}
