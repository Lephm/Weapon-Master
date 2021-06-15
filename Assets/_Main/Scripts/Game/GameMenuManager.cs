using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject escapeMenu;
    // Update is called once per frame
    void Update()
    {
        HandleEscapeMenu();
    }

    
    void HandleEscapeMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeMenuActivity();
        }
    }

    //this is so it get called by the continue button
    public void HandleEscapeMenuActivity()
    {
        escapeMenu.SetActive(!escapeMenu.activeInHierarchy);
    }

    public void LeaveGame()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("MainMenu");

    }
}
