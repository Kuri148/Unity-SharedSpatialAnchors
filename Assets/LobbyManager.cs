using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    //public GameObject lobbyPanel;
    //public GameObject roomPanel;
    public TextMeshProUGUI roomNameText;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform roomListContent;

    public float timeBetweenRoomUpdates = 1.5f;
    float nextUpdateTime;

    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        Debug.Log("On Click Create");
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions { MaxPlayers = 4 });
        }
    }

    // Update is called once per frame
    public override void OnJoinedRoom()
    {
        roomNameText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            Debug.Log("Updating Room List");
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenRoomUpdates;
        }
        
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo info in roomList)
        {
            RoomItem newItem = Instantiate(roomItemPrefab, roomListContent);
            newItem.SetRoomName(info.Name);
            roomItemsList.Add(newItem);
        }
    }   
}
 