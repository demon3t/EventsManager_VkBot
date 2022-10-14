﻿using EventsLogic;
using EventsLogic.DatabaseRequest;
using System;
using System.Linq;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model.RequestParams;
using EventsLogic.HelperClasses;

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
            Console.WriteLine($"{DateTime.Now}: Bot started");                   // оповещение запуска бота

            Person.Admins = UsersDatabase.FindUsers(isAdmin: true);              // загрузка списка Адмистроторов и Помощников
            Event.ActualEvents = EventsDatabase.FindEvents(isActual: true);

            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {

            if (UsersDatabase.CheckUserToId(e.Message.PeerId.ToString()))
            {
                RegisterUser(e);
            }

            var person = UsersDatabase.FindUsers(id: e.Message.PeerId.ToString()).First();

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {person.Surname} {person.Name} {person.Id} : {e.Message.Text}");

            switch (person.Major)
            {
                case 0:
                    StatusLogic.FirstOccurrence(person, e); // ready
                    return;
                case 1:
                    StatusLogic.MainMenu(person, e); // ready
                    return;
                case 2:
                    StatusLogic.LookEvents(person, e); // ready
                    return;
                case 3:

                    return;
                case 4:
                    StatusLogic.CreateEvent(person, e);  // ready
                    return;
                case 5:

                    return;
                case 6:
                    StatusLogic.LookEvent(person, e); // ready
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
