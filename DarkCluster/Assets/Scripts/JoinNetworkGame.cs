using UnityEngine;
using System.Collections;

public class JoinNetworkGame : MonoBehaviour {

    private string _roomName = "Room";

	void Start () {
        PhotonNetwork.ConnectUsingSettings("v0.1");
	}
	
	void Update () {
	
	}

    void OnJoinedLobby()
    {
        Debug.Log("Joined lobby!");
        PhotonNetwork.JoinOrCreateRoom(_roomName, null, PhotonNetwork.lobby);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined room!");

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate("NetworkLogic", Vector3.zero, Quaternion.identity, 0);
        }
    }
}
