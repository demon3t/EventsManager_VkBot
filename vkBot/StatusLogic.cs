﻿using EventsLogic;
using System;
using System.Collections.Generic;
using System.Text;
using VkBotFramework.Models;

namespace vkBot
{
    static internal class StatusLogic
    {
        private enum Major
        {
            FirstOccurrence = 0,
            Normal = 1,
            MakeEvent = 2,
        }

        internal static void FirstOccurrence(Person person, MessageReceivedEventArgs e)
        {
            MessageConstructor.FirstOccurrence(person, e);

            DatebaseLogic.UserSetParams(person, major: 1, minor: 0);
        }
    }
}
