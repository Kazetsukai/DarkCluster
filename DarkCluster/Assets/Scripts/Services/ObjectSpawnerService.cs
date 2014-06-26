using Assets.Scripts.Events;
using Caliburn.Micro;
using DarkCluster.Core;
using DarkCluster.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ObjectSpawnerService : IService, IHandle<SpawnObjectEvent>
    {
        public void Handle(SpawnObjectEvent message)
        {
            var obj = Resources.Load<GameObject>(message.ObjectName);

            if (obj == null)
                throw new Exception("Couldn't find object named " + message.ObjectName + " to spawn.");

            Util.GetObjectTracker().TrackObject((GameObject)GameObject.Instantiate(obj, message.Position, Quaternion.identity), message.EventId);
        }
    }
}
