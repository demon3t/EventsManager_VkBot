using EventsLogic.Basic;
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
using static vkBot.Request.ClientRequest;
using static vkBot.Request.EventRequest;

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
            Client.Admins.AddRange(GetParams(isAdmin: true));                          // загрузка списка Адмистроторов и Помощников
            Event.ActualEvents.AddRange(GetParam(isActual: true));

            vkBot.OnMessageReceived += VkBot_OnMessageReceived;
            Console.WriteLine($"{DateTime.Now}: Bot started");                   // оповещение успешного запуска бота
        }

        private static void VkBot_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Client client = Client.Admins.First(x => x.Id == e.Message.PeerId);
            if (client is null && !Existence(out client, (long)e.Message.PeerId))
            {
                RegisterUser(e);
                client = Get((long)e.Message.PeerId);
            }

            Console.WriteLine($"{DateTime.Now.ToString().Replace(' ', '/')}  {client.Surname} {client.Name} {client.Id} : {e.Message.Text}");

            switch (client.Major)
            {
                case (int)Major.FirstOccurrence: // ready
                    {
                        Entry.Go(client, e);
                        return;
                    }
                case (int)Major.Normal: // ready
                    {
                        MainMenu.Go(client, e);
                        return;
                    }
                case (int)Major.ViewEvents: // ready
                    {
                        ViewEvents.Go(client, e);
                        return;
                    }
                case (int)Major.MyEvents:
                    {
                        return;
                    }
                case (int)Major.CreateEvent: // ready
                    {
                        EventEditor.Go(client, e);
                        return;
                    }
                case (int)Major.EdingEvent: // ready
                    {
                        EventEditor.Go(client, e);
                        return;
                    }
                case (int)Major.PickedEvent: // ready
                    {
                        PickedEvent.Go(client, e);
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
            Add((long)e.Message.PeerId,
                vkBot.Api.Messages.GetConversations(param).Profiles[0].FirstName,
                vkBot.Api.Messages.GetConversations(param).Profiles[0].LastName, false);
        }

    }
}
