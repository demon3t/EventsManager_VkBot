using EventsLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
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

            adminList = DatebaseLogic.FillAdminList();                // загрузка списка Адмистроторов и Помощников
            DatebaseLogic.FillActualEvents(out Event.ActualEvents);  // загрузка списка актуальных мероприятий

            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (DatebaseLogic.CheckUserToId(e.Message.PeerId.ToString())) RegisterUser(e); // регистрация навого пользователя

            var person = DatebaseLogic.FindUsers(UserFindBy.Id, e.Message.PeerId).First();

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {person.SurName} {person.Name} {person.Id} : {e.Message.Text}");

            switch (person.Major)
            {
                case 0:
                    StatusLogic.FirstOccurrence(person,e);
                    break;
                case 1:
                    StatusLogic.FirstOccurrence(person, e);
                    break;
                case 2:
                    StatusLogic.FirstOccurrence(person, e);
                    break;
                case 3:
                    StatusLogic.FirstOccurrence(person, e);
                    break;
                case 4:
                    StatusLogic.FirstOccurrence(person, e);
                    break;
            }
            
            
            //switch (e.Message.Text.ToUpper())
            //{
            //    case "НАЧАТЬ":
            //    case "ПРИВЕТ":
            //        {
            //            MessageConstructor.ButtonBegin(e);
            //            break;
            //        }
            //    case "ИНФОРМАЦИЯ":
            //        {
            //            MessageConstructor.ButtonInfo(e);
            //            break;
            //        }
            //    case "ОБО МНЕ":
            //        {
            //            MessageConstructor.ButtonAboutMe(e, person.IsAdmin);
            //            break;
            //        }
            //    case "Я ВСЁ ЗНАЮ":
            //    case "НАЗАД":
            //        {
            //            MessageConstructor.ButtonKnown(e, person.IsAdmin);
            //            break;
            //        }
            //    case "ПОСМОТРЕТЬ МЕРОПРИЯТИЯ":
            //        {
            //            MessageConstructor.ButtonLookEvents(e);
            //            break;
            //        }
            //    default:
            //        {
            //            break;
            //        }
            //}
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

        private static void ChoiseEvent(object sender, MessageReceivedEventArgs e, bool IsAdmin)
        {
            switch (e.Message.Text.ToUpper())
            {
                case "Я НЕ ПОЙДУ":
                    {
                        MessageConstructor.ButtonNotGo(e, IsAdmin);
                        break;
                    }
                case "Я ПОЙДУ":
                    {
                        MessageConstructor.ButtonGo(e, IsAdmin);
                        break;
                    }
                case "РЕДАКТИРОВАТЬ":
                    {
                        MessageConstructor.ButtunExitChoise(e, IsAdmin);
                        break;
                    }
                case "УДАЛИТЬ":
                    {
                        MessageConstructor.ButtunExitChoise(e, IsAdmin);
                        break;
                    }
                default:
                    {
                        MessageConstructor.ButtunExitChoise(e, IsAdmin);
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
            DatebaseLogic.AddUser(e.Message.PeerId.ToString(),
                vkBot.Api.Messages.GetConversations(param).Profiles[0].FirstName,
                vkBot.Api.Messages.GetConversations(param).Profiles[0].LastName, false);
        }

    }
}
