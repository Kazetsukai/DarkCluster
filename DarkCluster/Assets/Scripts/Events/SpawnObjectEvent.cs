using Assets.Scripts.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class SpawnObjectEvent : IEvent
    {
        public int EventId { get; set; }

        public string ObjectName { get; set; }
        public Vector3 Position { get; set; }
    }
}
