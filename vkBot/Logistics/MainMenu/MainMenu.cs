using EventsLogic.Basic;
using VkBotFramework.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using static vkBot.General.KeyboardGeneral;
using static vkBot.General.MessageGeneral;

namespace vkBot.Logistics.MainMenu
{
    internal static class MainMenu
    {
        internal static void Go(Client client, MessageReceivedEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "Посмотреть мероприятия":
                    {
                        ViewEvents.ViewEvents.Go(client, e);
                        return;
                    }
                case "Создать мероприятие":
                    {
                        EventEditor.EventEditor.Go(client, e);
                        return;
                    }
                case "Мои мероприятия":
                    {
                        return;
                    }
                case "Завершённые мероприятия":
                    {
                        return;
                    }
                case "Информация": // ready
                    {
                        Information(client, e);
                        return;
                    }
                case "Обо мне": // ready
                    {
                        AboutMe(client, e);
                        return;
                    }
                default: return;
            }
        }

        internal static void This(Client client, MessageReceivedEventArgs e)
        {
            string message =
                "Главное меню";

            SendMessage(e, message, KeyboardThis(client));
        }

        #region Response options

        private static void Information(Client client, MessageReceivedEventArgs e)
        {
            string message =
                "Информация о боте";

            SendMessage(e, message, KeyboardThis(client));
        }

        private static void AboutMe(Client client, MessageReceivedEventArgs e)
        {
            string message =
                client.About();

            SendMessage(e, message, KeyboardThis(client));
        }

        #endregion

        internal static KeyboardBuilder KeyboardThis(Client client)
        {
            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()

                .AddButton("Посмотреть мероприятия", "", Positive)
                .AddLine();

            if (client.IsAdmin)
                keyboard
                    .AddButton("Создать мероприятие", "", Primary).AddLine();

            keyboard
                .AddButton("Мои мероприятия", "", Primary)
                .AddButton("Завершённые мероприятия", "", KeyboardButtonColor.Default).AddLine()
                .AddButton("Информация", "", Default)
                .AddButton("Обо мне", "", Default);

            return keyboard;
        }
    }
}
