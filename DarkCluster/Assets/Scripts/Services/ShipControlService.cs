using Assets.Scripts.Events;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DarkCluster.Core.Services
{
    public class ShipControlService : IService, IHandle<ShipControllingPlayerChangedEvent>
    {
        public void Handle(ShipControllingPlayerChangedEvent message)
        {
            Debug.Log("Roger that!");

            IEnumerable<PlayerShipController> otherControllers = GameObject.FindObjectsOfType<PlayerShipController>();
            foreach (var controller in otherControllers)
            {
                GameObject.Destroy(controller.gameObject);
            }

            var findShip = GameObject.Find("StarShip(Clone)");
            var objectTracker = GameObject.FindObjectOfType<ObjectLocationTracker>();
            
            if (findShip != null)
            {
                if (message.PlayerID != -1)
                {
                    objectTracker.PauseTrackingObject(findShip);
                    //Debug.Log("I am player " + PhotonNetwork.player.ID + " and player " + message.PlayerID + " is taking command.");
                    if (message.PlayerID == 0)//PhotonNetwork.player.ID)
                    {
                        var ship = GameObject.Instantiate(Resources.Load<MonoBehaviour>("PlayerShipController"));
                    }
                }
                else
                {
                    objectTracker.UnpauseTrackingObject(findShip);
                }
            }
        }
    }
}
