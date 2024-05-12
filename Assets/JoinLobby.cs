using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinLobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI buttonText;


    public void OnClickConnect()
    {
        if (playerNameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = playerNameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
