using EventsLogic.Basic;
using EventsLogic.DatabaseRequest;
using System;
using System.Linq;
using vkBot.Logistics;
using vkBot.Logistics.MainMenu;
using vkBot.Logistics.MainMenu.EventEditor;
using vkBot.Logistics.MainMenu.ViewEvents;
using vkBot.Logistics.MainMenu.ViewEvents.PickedEvent;
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
        public readonly static VkBot vkBot = new VkBot(config.Token, config.URL);

        public enum Major
        {
            FirstOccurrence = 0,
            Normal = 1,

            ViewEvents = 2,
            PickedEvent = 6,

            MyEvents = 3,

            CreateEvent = 4,
            EdingEvent = 5,
        }


        static void Main(string[] args)
        {
            vkBot.OnBotStarted += VkBot_OnBotStarted;

            vkBot.Start();
        }

        private static void VkBot_OnBotStarted(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now}: Bot started");                   // оповещение запуска бота

            Client.Admins = ClientDatabase.FindUsers(isAdmin: true);              // загрузка списка Адмистроторов и Помощников
            Event.ActualEvents = EventsDatabase.FindEvents(isActual: true);

            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {

            if (ClientDatabase.CheckUserToId(e.Message.PeerId.ToString()))
            {
                RegisterUser(e);
            }

            var person = ClientDatabase.FindUsers(id: e.Message.PeerId.ToString()).First();

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {person.Surname} {person.Name} {person.Id} : {e.Message.Text}");

            switch (person.Major)
            {
                case (int)Major.FirstOccurrence: // ready
                    {
                        Entry.Go(person, e);
                        return;
                    }
                case (int)Major.Normal: // ready
                    {
                        MainMenu.Go(person, e);
                        return;
                    }
                case (int)Major.ViewEvents: // ready
                    {
                        ViewEvents.Go(person, e);
                        return;
                    }
                case (int)Major.MyEvents:
                    {
                        return;
                    }
                case (int)Major.CreateEvent: // ready
                    {
                        EventEditor.Go(person, e);
                        return;
                    }
                case (int)Major.EdingEvent: // ready
                    {
                        EventEditor.Go(person, e);
                        return;
                    }
                case (int)Major.PickedEvent: // ready
                    {
                        PickedEvent.Go(person, e);
                        return;
                    }
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
            ClientDatabase.AddUser(e.Message.PeerId.ToString(),
                vkBot.Api.Messages.GetConversations(param).Profiles[0].FirstName,
                vkBot.Api.Messages.GetConversations(param).Profiles[0].LastName, false);
        }

    }
}
