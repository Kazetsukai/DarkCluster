using UnityEngine;
using System.Collections;
using DarkCluster.Core;

public class ThirdPersonShipCamera : MonoBehaviour {

    private GameObject _ship;

    private Camera _camInterior;
    private Camera _camExterior;

	// Use this for initialization
	void Start () {
        _camInterior = GameObject.Find("Interior Camera").camera;
        _camExterior = GameObject.Find("Exterior Camera").camera;
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            if (_camExterior.enabled)
            {
                _camInterior.enabled = true;
                _camExterior.enabled = false;
            }
            else
            {
                _camExterior.enabled = true;
                _camInterior.enabled = false;
            }
        }
    }

	void LateUpdate () {

        if (_ship != null)
        {
            _camExterior.transform.position = _ship.transform.position + (_ship.transform.localRotation * (Vector3.back * 2));
            _camExterior.transform.LookAt(_ship.transform.position, _ship.transform.localRotation * Vector3.up);

            _camInterior.transform.position = _ship.transform.position;
            _camInterior.transform.LookAt(_ship.transform.position + (_ship.transform.localRotation * Vector3.forward), _ship.transform.localRotation * Vector3.up);
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
