using UnityEngine;
using System.Collections;
using System;

public class NetworkPhysicsManager : MonoBehaviour {

    GameObject _enemy;

	// Use this for initialization
	void Start() 
    {
	    
	}
	
	// Update is called once per frame
	void Update()
    {
        if (_enemy == null)
            _enemy = (GameObject)Instantiate(Resources.Load<GameObject>("EnemyShip"), Vector3.forward * 5, Quaternion.identity);
        else if (PhotonNetwork.isMasterClient)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            var pPos = player.transform.position;
            var ePos = _enemy.transform.position;

            // Get direction between enemy and player
            var dir = pPos - ePos;
            dir.Normalize();
            
            // Get difference between current heading/speed and ideal heading/speed
            var delta = dir - _enemy.rigidbody.velocity;

            // Move that way
            _enemy.rigidbody.AddForce(delta);

            // And point that way
            _enemy.transform.rotation = Quaternion.LookRotation(delta);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (_enemy == null)
                stream.SendNext(0);
            else
            {
                stream.SendNext(1);
                EnemyData.Send(new EnemyData()
                {
                    position = _enemy.transform.position,
                    velocity = _enemy.rigidbody.velocity,
                    rotation = _enemy.transform.rotation,
                    angularVelocity = _enemy.rigidbody.angularVelocity
                }, stream);
            }
        }
        else
        {
            var enemyCount = (int)stream.ReceiveNext();
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = EnemyData.Receive(stream);
                
                if (_enemy != null)
                {
                    _enemy.transform.position = enemy.position;
                    _enemy.rigidbody.velocity = enemy.velocity;
                    _enemy.transform.rotation = enemy.rotation;
                    _enemy.rigidbody.angularVelocity = enemy.angularVelocity;
                }
            }
        }
    }

    public struct EnemyData
    {
        public Vector3 position;
        public Vector3 velocity;
        public Quaternion rotation;
        public Vector3 angularVelocity;

        internal static EnemyData Receive(PhotonStream stream)
        {
            return new EnemyData()
            {
                position = (Vector3)stream.ReceiveNext(),
                velocity = (Vector3)stream.ReceiveNext(),
                rotation = (Quaternion)stream.ReceiveNext(),
                angularVelocity = (Vector3)stream.ReceiveNext(),
            };
        }
        internal static void Send(EnemyData me, PhotonStream stream)
        {
            stream.SendNext(me.position);
            stream.SendNext(me.velocity);
            stream.SendNext(me.rotation);
            stream.SendNext(me.angularVelocity);
        }
    }
}