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
    public class LaserService : IService, IHandle<LaserFiredEvent>
    {
        public void Handle(LaserFiredEvent message)
        {
            Debug.Log("Laser fire handling...");
            var target = Util.GetObjectTracker().Get(message.TargetID);
            var ship = Util.GetShip();

            if (target != null && ship != null)
            {
                FireMahLaser(ship, target);
            }
        }

        private void FireMahLaser(GameObject from, GameObject to)
        {
            Debug.Log("Fire mah laser!");
            var laser = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("LaserBeam"));

            var laserScript = laser.GetComponent<LaserScript>();
            laserScript.Origin = from;
            laserScript.Target = to;
        }
    }
}
