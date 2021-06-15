using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class JoinRoomPanel : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI privateRoomNameInput;
    public GameObject roomDisplayPrefab;
    private List<GameObject> roomdisplayObjectsCache;
    public GameObject roomDisplayHolder;
    void Awake()
    {
        roomdisplayObjectsCache = new List<GameObject>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshRoomList();
    }
    public void RefreshRoomList()
    {
        PhotonNetwork.JoinLobby();
    }
    public void JoinPrivateRoom()
    {
        PhotonNetwork.JoinRoom(privateRoomNameInput.text);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        print("Update List is called");
        if (roomdisplayObjectsCache == null) return;
        //delete all room display prefabs
        print("Delete all cache");
        foreach(GameObject roomDisplay in roomdisplayObjectsCache.ToArray())
        {   
            if(roomdisplayObjectsCache.Contains(roomDisplay))
            {
                roomdisplayObjectsCache.Remove(roomDisplay);
            }
            Destroy(roomDisplay.gameObject);
        }
        print("Create new List");
        //Instatiate new room display prefab
        List<GameObject> newRoomList = new List<GameObject>();
        foreach (RoomInfo room in roomList)
        {   
            if(!room.IsOpen)
            {
                continue;
            }
            GameObject roomDisplayObject = Instantiate(roomDisplayPrefab, roomDisplayHolder.transform);
            newRoomList.Add(roomDisplayObject);
            RoomDisplay roomDisplay = roomDisplayObject.GetComponent<RoomDisplay>();
            if(roomDisplay != null)
            {
                roomDisplay.Setup(room);
            }
        }

        roomdisplayObjectsCache.AddRange(newRoomList);
    }
}
