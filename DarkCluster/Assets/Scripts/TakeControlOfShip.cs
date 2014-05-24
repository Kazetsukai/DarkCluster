using UnityEngine;
using System.Collections;
using Assets.Scripts.Events;

public class TakeControlOfShip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("Taking command...");
            ((NetworkEventAggregator)UnityEngine.Object.FindObjectOfType(typeof(NetworkEventAggregator))).Publish(new ShipControllingPlayerChangedEvent() { PlayerID = PhotonNetwork.player.ID });
        }
	}
}
