using UnityEngine;
using System.Collections;

public class TrackedObject : MonoBehaviour {

    public int ID { get; set; }

    //public TrackedObjectFrame[] oldFrames;
    public double LatestTimestamp;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	}
}

public struct TrackedObjectFrame
{
    public double timeStamp;

    public Vector3 position;
    public Quaternion rotation;

    public Vector3 velocity;
    public Vector3 angularVelocity;
}
