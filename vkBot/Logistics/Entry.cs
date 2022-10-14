using System;
using EventsLogic.Basic;
using VkBotFramework.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using static vkBot.General.KeyboardGeneral;
using static vkBot.General.MessageGeneral;
using static vkBot.Program;

namespace vkBot.Logistics
{
    internal static class Entry
    {
        internal static void Go(Client client, MessageReceivedEventArgs e)
        {
            Message(client, e);
            client.Major = (int)Major.Normal;
            client.Minor = 0;
        }

        private static void Message(Client client, MessageReceivedEventArgs e)
        {
            string message =
                $"Привет {client.Name}. Данный бот может:{Environment.NewLine}" +
                $"➤  Записывать на доступные мероприятия;{Environment.NewLine}" +
                $"➤  Заранее оповещать о начале мероприятий;{Environment.NewLine}" +
                $"➤  Показать посещённые мероприяти;{Environment.NewLine}" +
                $"➤  Показывать статистику.";

            SendMessage(e, message, MainMenu(client.IsAdmin));
        }

        private static KeyboardBuilder MainMenu(bool IsAdmin)
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

    }
}
