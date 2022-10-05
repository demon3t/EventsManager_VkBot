using System;
using VkBotFramework;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using VkBotFramework.Models;

namespace EventsLogic
{
    static public class KeyboardConstructor
    {
        public static VkBot? vkBot { private get; set; }

        public static string connectionDbString { private get; set; } = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security=True";

        public static void ButtonBegin(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Информация", "", KeyboardButtonColor.Default)
                .AddButton("Я всё знаю", "", KeyboardButtonColor.Positive).SetOneTime();

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = "Привет, уже был здесь?",
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build()
            });
        }

        public static void ButtonInfo(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Я всё знаю", "", KeyboardButtonColor.Positive).SetOneTime();

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = $"\tПривет {1}.Данный бот может оповещать и записывать на доступные мероприятия." +
                    "А так же напоминать о мероприятиях на которые Вы записались.",
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build()
            });
        }

        public static void ButtonKnown(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Посмотреть мероприятия", "", KeyboardButtonColor.Positive)
                .AddLine()
                .AddButton("Мои мероприятия", "", KeyboardButtonColor.Primary)
                .AddButton("Завершённые мероприятия", "", KeyboardButtonColor.Default).SetOneTime();

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = "Тогда продолжим.\nЧто ты хочешь сделать?",
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build()
            });
        }

        public static void ButtonLookEvents(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            LookEventsButtons(out KeyboardBuilder keyboard);

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = "Вот все доступные мероприятия",
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build()
            });
        }

        public static void CheckEvents(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;
        }

        private static KeyboardBuilder LookEventsButtons(out KeyboardBuilder keyboard)
        {
            keyboard = new KeyboardBuilder();
            string buttonName = "Some Event";

            using (SqlConnection connection = new SqlConnection(connectionDbString))
            {
                connection.Open();
                buttonName = connection.Database;
            }
            keyboard.AddButton(buttonName.Substring(0, 40), "", KeyboardButtonColor.Default).SetOneTime();

            return keyboard;
        }

    }
}
