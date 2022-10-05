using EventsLogic;
using System;
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

        static void Main(string[] args)
        {
            KeyboardConstructor.vkBot = vkBot;
            vkBot.OnBotStarted += VkBot_OnBotStarted;
            

            vkBot.Start();
        }

        private static void VkBot_OnBotStarted(object sender, EventArgs e)
        {
            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
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
                        KeyboardConstructor.CheckEvents(e);
                        break;
                    }
            }
            Console.WriteLine($"{e.Message.UpdateTime}: {e.Message.Text}");
        }
    }
}
