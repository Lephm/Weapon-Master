using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviourPunCallbacks
{

    public GameObject[] menuPanels;
    public TextMeshProUGUI roomNameInput;
    public TextMeshProUGUI playerNameInput;
    public TextMeshProUGUI errorMessage;
    public TextMeshProUGUI roomPrivacyText;
    public bool roomIsPublic = true;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // This is To ensure that the player leave room when leave game
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public void ChangeMenu(int index)
    {
        for(int i = 0; i <= menuPanels.Length -1; i++)
        {
            menuPanels[i].SetActive(i == index);
        }
    }

    public void GoBackToMainMenu()
    {
       foreach(GameObject menu in menuPanels)
       {
            menu.SetActive(false);
       }
       if(menuPanels.Length >=2)
       {
            menuPanels[0].SetActive(true); //enable main menu
            menuPanels[1].SetActive(true); // enable character selection
       }
        
    }
   public void QuitGame()
   {
        Application.Quit();
   }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Room");
        }
        
    }

    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        DisplayErrorMessage();
        base.OnJoinRandomFailed(returnCode,message);
        
        
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = roomIsPublic;
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
    }

    public void JoinRandomRoom()
    {   

        PhotonNetwork.JoinRandomRoom();
    }

    void DisplayErrorMessage()
    {
        StopCoroutine("ErrorMessageDisplayAnimation");
        StartCoroutine("ErrorMessageDisplayAnimation");
    }

    IEnumerator ErrorMessageDisplayAnimation()
    {
        errorMessage.gameObject.SetActive(true);
        bool fade = true;
        while(fade)
        {
            errorMessage.alpha -= 0.1f;
            yield return new WaitForSeconds(0.8f);
            if(errorMessage.alpha <= 0.4f)
            {
                fade = false;
                errorMessage.gameObject.SetActive(false); 
            }

        }

    }

    public void ToogleRoomPrivacy()
    {
        roomIsPublic = !roomIsPublic;
        if(roomIsPublic)
        {
            roomPrivacyText.text = "Public";
        }
        else
        {
            roomPrivacyText.text = "Private";
        }
    }

    private void OnDestroy()
    {   
        //Set nick name when object is destroyed (new scene is loaded)
        PhotonNetwork.LocalPlayer.NickName = playerNameInput.text;
        print(PhotonNetwork.LocalPlayer.NickName + "Successfully join or create room");

    }


}
