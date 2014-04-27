using UnityEngine;
using System.Collections;
using System;

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
	void Update () {

        if (_ship != null)
        {
            //Camera.main.transform.Rotate(new Vector3(0.1f, 0.6f, 0.2f), -0.05f);

            var cam = Camera.main;
            cam.transform.position = _ship.transform.position + (_ship.transform.localRotation * (Vector3.back * 2));
            cam.transform.LookAt(_ship.transform.position, _ship.transform.localRotation * Vector3.up);

            var roll = -Input.GetAxis("Horizontal");
            var pitch = Input.GetAxis("Vertical");

            roll /= 10.0f;
            pitch /= 10.0f;

            var relativeAngularVelocity = _ship.transform.InverseTransformDirection(_ship.rigidbody.angularVelocity);
            _ship.rigidbody.AddRelativeTorque(pitch, -relativeAngularVelocity.y, roll);

            if (Input.GetButton("Fire1"))
                _ship.rigidbody.AddRelativeForce(Vector3.forward * 50);

            Debug.DrawRay(_ship.transform.position, _ship.rigidbody.velocity, Color.white);

            if (Input.GetButtonDown("Fire2"))
            {
                var enemy = GameObject.Find("EnemyShip(Clone)");
                if (enemy != null)
                    FireMahLaser(_ship, enemy);
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

    private void FireMahLaser(GameObject from, GameObject to)
    {
        var laser = (GameObject)Instantiate(Resources.Load<GameObject>("LaserBeam"));

        var laserScript = laser.GetComponent<LaserScript>();
        laserScript.Origin = from;
        laserScript.Target = to;
    }
}
