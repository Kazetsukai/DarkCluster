using UnityEngine;
using System.Collections;

public class UniverseScript : MonoBehaviour {

    GameObject _enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (_enemy == null)
        {
            _enemy = PhotonNetwork.Instantiate("EnemyShip", Vector3.forward * 5, Quaternion.identity, 0);
        }
	}
}
