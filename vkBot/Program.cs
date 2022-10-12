﻿using EventsLogic;
using EventsLogic.DatabaseRequest;
using System;
using System.Linq;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model.RequestParams;

namespace vkBot
{
    internal class Program
    {
        /// <summary>
        /// API VKBot
        /// </summary>
        private static VkBot vkBot = new VkBot(config.Token, config.URL);

        static void Main(string[] args)
        {
            MessageConstructor.vkBot = vkBot;
            vkBot.OnBotStarted += VkBot_OnBotStarted;

            vkBot.Start();
        }

        private static void VkBot_OnBotStarted(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now}: Bot started");                       // оповещение запуска бота
            Person.Admins = UsersDatabase.FindUsers(UserFindBy.Admin, true);         // загрузка списка Адмистроторов и Помощников
            Event.ActualEvents = EventsDatabase.FillActualEvents();                  // загрузка списка актуальных мероприятий

            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (UsersDatabase.CheckUserToId(e.Message.PeerId.ToString()))
            {
                RegisterUser(e);
            }

            var person = UsersDatabase.FindUsers(UserFindBy.Id, e.Message.PeerId).First();

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {person.SurName} {person.Name} {person.Id} : {e.Message.Text}");

            switch (person.Major)
            {
                case 0:
                    StatusLogic.FirstOccurrence(person, e);
                    return;
                case 1:
                    StatusLogic.MainMenu(person, e);
                    return;
                case 2:
                    StatusLogic.LookEvents(person, e);
                    return;
                case 3:

                    return;
                case 4:
                    StatusLogic.CreateEvent(person, e);
                    return;
                default: return;
            }
        }

        private static void RegisterUser(MessageReceivedEventArgs e)
        {
            GetConversationsParams param = new GetConversationsParams()
            {
                Count = 1,
                GroupId = (ulong?)e.Message.PeerId,
                Extended = true,
            };
            UsersDatabase.AddUser(e.Message.PeerId.ToString(),
                vkBot.Api.Messages.GetConversations(param).Profiles[0].FirstName,
                vkBot.Api.Messages.GetConversations(param).Profiles[0].LastName, false);
        }

    }
}
