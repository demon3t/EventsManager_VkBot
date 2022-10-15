using System;
using System.Collections.Generic;
using System.Text;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace vkBot.General
{
    internal static class MessageGeneral
    {
        private static VkBot _vkBot;
        static MessageGeneral()
        {
            _vkBot = Program.vkBot;
        }

        internal static void SendMessage(MessageReceivedEventArgs e, string message, KeyboardBuilder keyboard)
        {
            _vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                UserId = e.Message.PeerId,
                RandomId = Environment.TickCount,
                Keyboard = keyboard.Build(),
            });
        }

        internal static void SendMessage(MessageReceivedEventArgs e, string message)
        {
            _vkBot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = message,
                UserId = e.Message.PeerId,
                RandomId = Environment.TickCount,
            });
        }
    }
}
