using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roomNameText;

    public void SetRoomName(string _name)
    {
        roomNameText.text = _name;
    }
}
