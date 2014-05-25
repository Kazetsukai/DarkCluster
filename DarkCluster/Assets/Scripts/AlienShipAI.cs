using UnityEngine;
using System.Collections;

public class AlienShipAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody.AddForce(Random.insideUnitSphere);
	}
}
