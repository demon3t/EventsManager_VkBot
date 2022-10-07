using System;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace vkBot
{
    internal class MeInfo
    {
        internal static void AddNewUser(VkBot vkBot, MessageReceivedEventArgs e, User @profiles)
        {
            if (vkBot == null) return;
            string message =
                $"Зарегестрирован новый пользователь\n" +
                $"Имя               {@profiles.FirstName}\n" +
                $"Фамилия      {@profiles.LastName}\n" + 
                $"Id                    {@profiles.Id}\n" +
                $"PeerId            {e.Message.PeerId}\n" +
                $"{(@profiles.Id == e.Message.PeerId ? "GOOD" : "BAD")}";

            vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                PeerId = 163770663,
                RandomId = Environment.TickCount
            });
        }
    }
}
