using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
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

        /// <summary>
        /// Начальная клавиатура
        /// </summary>
        /// <returns></returns>
        internal static KeyboardBuilder KeyboardBegin()
        {
            return (KeyboardBuilder)new KeyboardBuilder()
                 .AddButton("Информация", "", KeyboardButtonColor.Default)
                 .AddButton("Я всё знаю", "", KeyboardButtonColor.Positive)
                 .SetOneTime();
        }


        /// <summary>
        /// Кнопка отвечающая за "Информация"
        /// </summary>
        /// <returns></returns>
        internal static KeyboardBuilder KeyboardInfo()
        {
            return (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Я всё знаю", "", KeyboardButtonColor.Positive)
                .SetOneTime();
        }

        /// <summary>
        /// Кнопка отвечающая за "Я всё знаю","Назад" и "Обо мне"
        /// </summary>
        /// <param name="IsAdmin"> Является ли пользователь аминистраторо? </param>
        /// <returns></returns>
        internal static KeyboardBuilder KeyboardKnown(bool IsAdmin)
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
            .AddButton("Обо мне", "", Default)
            .SetOneTime();

            return keyboard;
        }

        /// <summary>
        /// Кнопка отвечающая за "Посмотреть мероприятия"
        /// </summary>
        /// <param name="actualEvents"> Список актуальных мероприятий </param>
        /// <returns></returns>
        internal static KeyboardBuilder KeyboardLookEvent(MessageReceivedEventArgs e)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            Event.InterestUsers.Add(e.Message.PeerId.ToString());

            foreach (var _event in Event.ActualEvents)
            {
                if (_event.Name == null) continue;
                keyboard.AddButton(_event.Name.ToString(), "").AddLine();
            }

            keyboard
                .AddButton("Назад", "", KeyboardButtonColor.Primary)
                .SetOneTime();
            return keyboard;
        }

        internal static KeyboardBuilder KeyboardEvent(MessageReceivedEventArgs e, Event selectEvent, bool IsAdmin)
        {
            KeyboardBuilder keyboard = new KeyboardBuilder();

            if (selectEvent.InvolvedUsers.Contains(e.Message.PeerId.ToString()))
                keyboard
                    .AddButton("Я не пойду", "", Negative).AddLine();
            else
                if (selectEvent._Count < selectEvent.Count)
                    keyboard
                        .AddButton("Я пойду", "", Positive).AddLine();

            if (IsAdmin)
                keyboard
                    .AddButton("Редактировать", "", Primary)
                    .AddButton("Удалить", "", Negative)
                    .SetOneTime();

            return keyboard;
        }
    }
}
