using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class PlayerShipController : Photon.MonoBehaviour {
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
            // TODO: Find a nicer way to determine owner of an object
            if (IsOwner)
            {
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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            IsOwner = true;
            WriteTrackedObjectsTo(stream);
        }
        else
        {
            IsOwner = false;
            UpdateTrackedObjectsFrom(stream, info);
        }
    }

    private void UpdateTrackedObjectsFrom(PhotonStream stream, PhotonMessageInfo info)
    {
        var obj = GameObject.Find("StarShip(Clone)");

        var position = (Vector3)stream.ReceiveNext();
        var rotation = (Quaternion)stream.ReceiveNext();

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        var velocity = (Vector3)stream.ReceiveNext();
        var angularVelocity = (Vector3)stream.ReceiveNext();

        obj.rigidbody.velocity = velocity;
        obj.rigidbody.angularVelocity = angularVelocity;

        // Estimate position based on lag
        double timeLag = PhotonNetwork.time - info.timestamp;
        Debug.Log(timeLag);
        obj.transform.position += obj.rigidbody.velocity * (float)timeLag;
        obj.transform.rotation *= Quaternion.AngleAxis((float)timeLag, obj.rigidbody.angularVelocity);
    }

    private void WriteTrackedObjectsTo(PhotonStream stream)
    {
        var obj = GameObject.Find("StarShip(Clone)");

        var objTransform = obj.transform;
        var objRigidbody = obj.rigidbody;

        stream.SendNext(objTransform.position);
        stream.SendNext(objTransform.rotation);

        stream.SendNext(objRigidbody.velocity);
        stream.SendNext(objRigidbody.angularVelocity);

    }

    public bool IsOwner { get; set; }
}
