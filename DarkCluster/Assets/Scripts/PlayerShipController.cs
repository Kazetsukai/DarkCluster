using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class PlayerShipController : MonoBehaviour {
    private GameObject _ship;

	void Start () {
        
	}

    void OnGUI()
    {
        if (_ship != null)
            GUI.Label(new Rect(300, 100, 300, 30), "Velocity: " + Math.Round(_ship.rigidbody.velocity.magnitude, 4));
    }

	
	// Update is called once per frame
    void Update()
    {
        if (_ship != null)
        {
            //if (photonView.isMine)
            {
                var roll = -Input.GetAxis("Horizontal");
                var pitch = Input.GetAxis("Vertical");

                roll /= 2.0f;
                pitch /= 2.0f;

                var relativeAngularVelocity = _ship.transform.InverseTransformDirection(_ship.rigidbody.angularVelocity);
                _ship.rigidbody.AddRelativeTorque(pitch, -relativeAngularVelocity.y, roll);

                if (Input.GetButton("Fire1"))
                    _ship.rigidbody.AddRelativeForce(Vector3.forward * 50);

                Debug.DrawRay(_ship.transform.position, _ship.rigidbody.velocity, Color.white);
            }

        }
        else
        {
            var findShip = GameObject.Find("StarShip(Clone)");
            if (findShip != null)
            {
                _ship = findShip;
            }
        }
    }

    public bool IsOwner { get; set; }
}
