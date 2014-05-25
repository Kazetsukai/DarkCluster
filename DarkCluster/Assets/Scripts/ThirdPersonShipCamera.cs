using UnityEngine;
using System.Collections;
using DarkCluster.Core;

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
            var findShip = Util.GetShip();
            if (findShip != null)
            {
                _ship = findShip;
            }
        }
	}
}
