using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using DarkCluster.Core;
using Assets.Scripts.Events;
using Assets.Scripts.Network;

public class ObjectLocationTracker : MonoBehaviour {

    Dictionary<int, TrackedObject> _trackedObjects = new Dictionary<int,TrackedObject>();
    public bool ReestimatePosition;

	void Start() 
	{
		Instantiate(Resources.Load<GameObject>("StarShip"));
		Debug.Log("Created ship");
		Util.GetEventAggregator().Publish(new ShipControllingPlayerChangedEvent() { PlayerID = 0 });
	}
	
	void Update()
    {
	}

    public void TrackObject(GameObject obj, int id)
    {
        var trackedObj = obj.AddComponent<TrackedObject>();

        trackedObj.Id = id;
        _trackedObjects.Add(trackedObj.Id, trackedObj);
    }

    public void StopTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            _trackedObjects.Remove(trackedObj.Id);
        }
    }

    public GameObject Get(int id)
    {
        return _trackedObjects[id].gameObject;
    }

    public void PauseTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            trackedObj.Paused = true;
        }
    }

    public void UnpauseTrackingObject(GameObject obj)
    {
        var trackedObj = obj.GetComponent<TrackedObject>();
        if (trackedObj != null)
        {
            trackedObj.Paused = false;
        }
    }
}