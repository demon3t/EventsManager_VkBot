using System;
using VkBotFramework;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using VkBotFramework.Models;
using System.Threading;

namespace EventsLogic
{
    static public class KeyboardConstructor
    {
        public static VkBot? vkBot { private get; set; }
        public static string connectionDbString { private get; set; } = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Nipa\\source\\repos\\vkBot\\EventsLogic\\Database.mdf;Integrated Security=True";

        private static void SendMessage(MessageReceivedEventArgs e, string message, KeyboardBuilder keyboard)
        {
            if (vkBot == null) return;

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build()
            });
        }
        private static void SendMessage(MessageReceivedEventArgs e, string message)
        {
            if (vkBot == null) return;

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                PeerId = e.Message.PeerId,
                RandomId = Environment.TickCount,
            });
        }

        public static void ButtonBegin(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Информация", "", KeyboardButtonColor.Default)
                .AddButton("Я всё знаю", "", KeyboardButtonColor.Positive).SetOneTime();

            SendMessage(e, "Привет, уже был здесь?", keyboard);
        }

        public static void ButtonInfo(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Я всё знаю", "", KeyboardButtonColor.Positive).SetOneTime();

            SendMessage(e, $"\tПривет {1}.Данный бот может оповещать и записывать на доступные мероприятия. А так же напоминать о мероприятиях на которые Вы записались.", keyboard);
        }

        public static void ButtonKnown(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            KeyboardBuilder keyboard = (KeyboardBuilder)new KeyboardBuilder()
                .AddButton("Посмотреть мероприятия", "", KeyboardButtonColor.Positive)
                .AddLine()
                .AddButton("Мои мероприятия", "", KeyboardButtonColor.Primary)
                .AddButton("Завершённые мероприятия", "", KeyboardButtonColor.Default).SetOneTime();

            SendMessage(e, "Тогда продолжим.\nЧто ты хочешь сделать?", keyboard);
        }

        public static void ButtonLookEvents(MessageReceivedEventArgs e)
        {
            if (vkBot == null) return;

            LookEventsButtons(out KeyboardBuilder keyboard, e);



            SendMessage(e, "Вот все доступные мероприятия");
        }

        private static void LookEventsButtons(out KeyboardBuilder keyboard, MessageReceivedEventArgs e)
        {
            keyboard = (KeyboardBuilder)new KeyboardBuilder().AddButton("Я всё знаю","",KeyboardButtonColor.Primary);

            using (SqlConnection connection = new SqlConnection(connectionDbString))
            {
                connection.Open();
                SendMessage(e, $"\tСтрока подключения: {connection.ConnectionString}\n\tБаза данных: {connection.Database}\n\tСервер: {connection.DataSource}\n\tСостояние: {connection.State}");
                Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
                Console.WriteLine($"\tБаза данных: {connection.Database}");
                Console.WriteLine($"\tСервер: {connection.DataSource}");
                Console.WriteLine($"\tСостояние: {connection.State}");


            }
            Console.WriteLine("Подключение закрыто");
        }
    }
}
