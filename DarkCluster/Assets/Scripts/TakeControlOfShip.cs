using UnityEngine;
using System.Collections;
using Assets.Scripts.Events;
using DarkCluster.Core;

public class TakeControlOfShip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("Taking command...");
            Util.GetEventAggregator().Publish(new ShipControllingPlayerChangedEvent() { PlayerID = 0 });
        }
	}

}
