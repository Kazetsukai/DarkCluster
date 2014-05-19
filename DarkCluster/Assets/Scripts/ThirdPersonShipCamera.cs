using UnityEngine;
using System.Collections;

public class ThirdPersonShipCamera : MonoBehaviour {

    private GameObject _ship;

	// Use this for initialization
	void Start () {
	
	}
	
	void LateUpdate () {

        if (_ship != null)
        {
            var cam = Camera.main;
            cam.transform.position = _ship.transform.position + (_ship.transform.localRotation * (Vector3.back * 2));
            cam.transform.LookAt(_ship.transform.position, _ship.transform.localRotation * Vector3.up);
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
}
