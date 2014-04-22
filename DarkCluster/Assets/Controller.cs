using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    private GameObject _ship;
    private UniverseScript _universe;

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("v0.1");

        PhotonNetwork.offlineMode = true;
	}

    void OnJoinedLobby()
    {
        Debug.Log("Joined lobby!");
        PhotonNetwork.JoinOrCreateRoom("TheRoom", null, PhotonNetwork.lobby);
    }
    
    void OnJoinedRoom()
    {
        Debug.Log("Joined room!");

        _ship = PhotonNetwork.Instantiate("StarShip", Vector3.zero, Quaternion.identity, 0);
        
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate("Network", Vector3.zero, Quaternion.identity, 0);
        }
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

            _ship.rigidbody.AddRelativeTorque(pitch, 0, roll);

            if (Input.GetButton("Fire1"))
                _ship.rigidbody.AddRelativeForce(Vector3.forward * 50);

            Debug.DrawRay(_ship.transform.position, _ship.rigidbody.velocity, Color.white);
        }
	}
}
