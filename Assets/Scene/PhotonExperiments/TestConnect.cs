using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {;
        Debug.Log("Connecting to server");
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        
        Debug.Log("Disconnected from server for reason: " + cause.ToString());
    }   
}
