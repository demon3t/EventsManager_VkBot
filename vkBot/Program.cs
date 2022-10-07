using EventsLogic;
using System;
using System.Collections.Generic;
using System.Reflection;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace vkBot
{
    internal class Program
    {
        #region локальные переменные

        /// <summary>
        /// API VKBot
        /// </summary>
        private static VkBot vkBot = new VkBot(config.Token, config.URL);

        /// <summary>
        /// Список администраторов
        /// </summary>
        private static List<string> adminList = new List<string>();

        /// <summary>
        /// Список администраторов создающих мероприятие
        /// </summary>
        private static List<string> makeList = new List<string>();

        /// <summary>
        /// Список помощников
        /// </summary>
        private static List<string> markList = new List<string>();

        #endregion

        static void Main(string[] args)
        {
            MessageConstructor.vkBot = vkBot;
            vkBot.OnBotStarted += VkBot_OnBotStarted;

            vkBot.Start();
        }

        private static void VkBot_OnBotStarted(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now}: Bot started");       // оповещение запуска бота
            DatabaseLogic.FillPriority(out adminList, out markList); // загрузка списка Адмистроторов и Помощников
            DatabaseLogic.FillActualEvents(out Event.ActualEvents);  // загрузка списка актуальных мероприятий
            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            bool IsAdmin = adminList.Contains(e.Message.PeerId.ToString());

            if (!DatabaseLogic.CheckUserToId(e.Message.PeerId.ToString())) RegisterUser(e); // регистрация навого пользователя

            if (makeList.Contains(e.Message.PeerId.ToString())) MakeEvent(sender, e);       // если пользователь создаёт меропряитие

            if (Event.InterestUsers.Contains(e.Message.PeerId.ToString())) SelectEvent(sender, e, IsAdmin);

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {e.Message.PeerId}:  {e.Message.Text}");

            switch (e.Message.Text.ToUpper())
            {
                case "НАЧАТЬ":
                case "ПРИВЕТ":
                    {
                        MessageConstructor.ButtonBegin(e);
                        break;
                    }
                case "ИНФОРМАЦИЯ":
                    {
                        MessageConstructor.ButtonInfo(e);
                        break;
                    }
                case "ОБО МНЕ":
                    {
                        MessageConstructor.ButtonAboutMe(e, IsAdmin);
                        break;
                    }
                case "Я ВСЁ ЗНАЮ":
                case "НАЗАД":
                    {
                        MessageConstructor.ButtonKnown(e, IsAdmin);
                        break;
                    }
                case "ПОСМОТРЕТЬ МЕРОПРИЯТИЯ":
                    {
                        MessageConstructor.ButtonLookEvents(e);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private static void SelectEvent(object sender, MessageReceivedEventArgs e, bool IsAdmin)
        {
            foreach (var _event in Event.ActualEvents)
            {
                if (_event.Name == e.Message.Text)
                {
                    MessageConstructor.BullonEvent(e, IsAdmin, _event);
                    return;
                }
            }
        }

        private static void MakeEvent(object sender, MessageReceivedEventArgs e)
        {

        }

        private static void RegisterUser(MessageReceivedEventArgs e)
        {
            GetConversationsParams param = new GetConversationsParams()
            {
                Count = 1,
                GroupId = (ulong?)e.Message.PeerId,
                Extended = true,
            };
            DatabaseLogic.AddUser(e.Message.PeerId.ToString(),
                vkBot.Api.Messages.GetConversations(param).Profiles[0].FirstName,
                vkBot.Api.Messages.GetConversations(param).Profiles[0].LastName, false);

            MeInfo.AddNewUser(vkBot, e, vkBot.Api.Messages.GetConversations(param).Profiles[0]);
        }

    }
}
