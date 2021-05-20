using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomLobby : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerStatusTextDisplay;
    public TextMeshProUGUI playerCountTextDisplay;
    public GameObject startButton;
    public GameObject backButton;

    private void Start()
    {
        UpdateUI();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdateUI();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("MainMenu");
        
    }
    public void UpdateUI()
    {
        playerCountTextDisplay.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        
        if(PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            startButton.SetActive(false);
            playerStatusTextDisplay.text = "Waiting for players";
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
                playerStatusTextDisplay.text = "Press Start To Play";
            }

            else
            {
                startButton.SetActive(false);
                playerStatusTextDisplay.text = "Waiting For Host to Start";
            }
        }

        
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(RoomSelection.currentLevelSelectionName);
    }

    public void BackToMainMenu()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    
}
