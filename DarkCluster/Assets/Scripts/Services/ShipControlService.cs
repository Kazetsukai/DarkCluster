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

            IEnumerable<PlayerShipController> otherControllers = GameObject.FindObjectsOfType<PlayerShipController>().Where(c => c.photonView.isMine);
            foreach (var controller in otherControllers)
            {
                PhotonNetwork.Destroy(controller.gameObject);
            }

            var findShip = GameObject.Find("StarShip(Clone)");
            var objectTracker = GameObject.FindObjectOfType<ObjectLocationTracker>();
            
            if (findShip != null)
            {
                if (message.PlayerID != -1)
                {
                    objectTracker.PauseTrackingObject(findShip);
                    Debug.Log("I am player " + PhotonNetwork.player.ID + " and player " + message.PlayerID + " is taking command.");
                    if (message.PlayerID == PhotonNetwork.player.ID)
                    {
                        var ship = PhotonNetwork.Instantiate("PlayerShipController", Vector3.zero, Quaternion.identity, 0);
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
