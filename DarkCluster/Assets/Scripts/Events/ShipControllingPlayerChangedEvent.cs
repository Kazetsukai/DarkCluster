﻿using Assets.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public class ShipControllingPlayerChangedEvent : IEvent
    {
        public int EventId { get; set; }

        public int PlayerID { get; set; }
    }
}
