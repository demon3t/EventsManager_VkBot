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
        private static VkBot vkBot = new VkBot(config.Token, config.URL);
        private static List<string> adminList = new List<string>();
        private static List<string> makeList = new List<string>();
        private static List<string> markList = new List<string>();
        private static List<Event> actualEvents = new List<Event>();
        static void Main(string[] args)
        {
            KeyboardConstructor.vkBot = vkBot;
            vkBot.OnBotStarted += VkBot_OnBotStarted;


            vkBot.Start();
        }

        private static void VkBot_OnBotStarted(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now}: Bot started");
            DatabaseLogic.FillPriority(out adminList, out makeList, out markList);
            DatabaseLogic.FillActualEvents(out actualEvents);
            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (!DatabaseLogic.CheckUserToId(e.Message.PeerId.ToString())) RegisterUser(e);

            if (makeList.Contains(e.Message.PeerId.ToString())) MakeEvent(sender, e);

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {e.Message.PeerId}:  {e.Message.Text}");

            switch (e.Message.Text.ToUpper())
            {
                case "НАЧАТЬ":
                case "ПРИВЕТ":
                    {
                        KeyboardConstructor.ButtonBegin(e);
                        break;
                    }
                case "ИНФОРМАЦИЯ":
                    {
                        KeyboardConstructor.ButtonInfo(e);
                        break;
                    }
                case "ОБО МНЕ":
                    {
                        KeyboardConstructor.ButtonAboutMe(e);
                        break;
                    }
                case "Я ВСЁ ЗНАЮ":
                case "НАЗАД":
                    {
                        KeyboardConstructor.ButtonKnown(e);
                        break;
                    }
                case "ПОСМОТРЕТЬ МЕРОПРИЯТИЯ":
                    {
                        KeyboardConstructor.ButtonLookEvents(e, actualEvents);
                        break;
                    }
                default:
                    {
                        break;
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

            MeInfo.AddNewUser(vkBot,e, vkBot.Api.Messages.GetConversations(param).Profiles[0]);
        }

    }
}
