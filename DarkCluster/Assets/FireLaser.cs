using UnityEngine;
using System.Collections;
using DarkCluster.Core;
using Assets.Scripts.Events;

public class FireLaser : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attempting to fire laser");
            var ship = Util.GetShip();
            var tracker = Util.GetObjectTracker();
            

            if (ship != null)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		        RaycastHit rayHit;

                if (Physics.Raycast(ray, out rayHit))
                {
                    Debug.Log("Found something...");
                    var tracked = rayHit.rigidbody.GetComponent<TrackedObject>();
                    if (tracked != null)
                    {
                        Debug.Log("Tracked object! Publishing laser fire!");
                        Util.GetEventAggregator().Publish(new LaserFiredEvent() { TargetID = tracked.ID });
                    }
                }
            }
        }
	}
}
