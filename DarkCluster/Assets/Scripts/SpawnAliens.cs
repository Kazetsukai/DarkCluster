using UnityEngine;
using System.Collections;
using DarkCluster.Core;
using Assets.Scripts.Events;

public class SpawnAliens : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        var eventAggregator = Util.GetEventAggregator();

	    for (int i = 0; i < 20; i++)
        {
            eventAggregator.Publish(new SpawnObjectEvent() { ObjectName = "EnemyShip", Position = Random.insideUnitSphere * 30 });
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
