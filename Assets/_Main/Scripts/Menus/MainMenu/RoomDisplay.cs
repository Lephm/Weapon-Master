using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class RoomDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roomNameDisplay;
    [SerializeField] TextMeshProUGUI roomPlayerNumDisplay;
    private RoomInfo roomInfo;

    public void Setup(RoomInfo room)
    {
        roomInfo = room;
        roomNameDisplay.text = roomInfo.Name;
        roomPlayerNumDisplay.text = roomInfo.PlayerCount.ToString() + "/" + roomInfo.MaxPlayers;
    }

    public void JoinRoom()
    {
        if(roomInfo == null)
        {
            return;
        }

        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
    
}
