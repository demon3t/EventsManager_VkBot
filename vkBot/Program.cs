using EventsLogic;
using System;
using System.Collections.Generic;
using System.Reflection;
using VkBotFramework;
using VkBotFramework.Models;
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
            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (!DatabaseLogic.CheckUserToId(e.Message.PeerId.ToString()))
                DatabaseLogic.AddUser(e.Message.PeerId.ToString(), "User", "Subuser", false);

            if (makeList.Contains(e.Message.PeerId.ToString()))
                MakeEvent(sender, e);

            Console.WriteLine($"{DateTime.Now}_{e.Message.PeerId}: {e.Message.Text}");
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
                    {
                        KeyboardConstructor.ButtonKnown(e);
                        break;
                    }
                case "ПОСМОТРЕТЬ МЕРОПРИЯТИЯ":
                    {
                        KeyboardConstructor.ButtonLookEvents(e);
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
    }
}
