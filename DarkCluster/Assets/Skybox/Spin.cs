using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
    private GameObject _ship;

	// Use this for initialization
	void Start () {
        _ship = GameObject.Find("AstraShuttlePrefab");
	}
	
	// Update is called once per frame
	void Update () {
        //Camera.main.transform.Rotate(new Vector3(0.1f, 0.6f, 0.2f), -0.05f);

        var cam = Camera.main;
        cam.transform.position = _ship.transform.position + (_ship.transform.localRotation * (Vector3.back * 2));
        cam.transform.LookAt(_ship.transform.position, _ship.transform.localRotation * Vector3.up);

        var roll = -Input.GetAxis("Horizontal");
        var pitch = Input.GetAxis("Vertical");

        _ship.rigidbody.AddRelativeTorque(pitch, 0, roll);

        if (Input.GetButton("Fire1"))
            _ship.rigidbody.AddRelativeForce(Vector3.forward * 50);
	}
}
