using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;

[Serializable]
public struct LevelInfo
{   
    public string levelName;
    public Sprite levelImage;
}

public class RoomSelection : MonoBehaviour
{
    public static string currentLevelSelectionName;
    public LevelInfo[] levels;
    public PhotonView view;
    public int currentLevelSelectionIndex = 0;
    public Image roomImageDisplay;
    public Text roomNameDisplay;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        currentLevelSelectionName = levels[currentLevelSelectionIndex].levelName;
        if(levels[currentLevelSelectionIndex].levelImage != null)
        {
            roomImageDisplay.sprite = levels[currentLevelSelectionIndex].levelImage;
        }
    }

    [PunRPC]
    public void UpdateUIRPC(int currentSelectionIndex)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            currentLevelSelectionIndex = currentSelectionIndex;
        }

        UpdateUI();
        
    }
    public void NextLevel()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        currentLevelSelectionIndex = (currentLevelSelectionIndex + 1) % levels.Length;
        view.RPC("UpdateUIRPC", RpcTarget.AllBuffered,currentLevelSelectionIndex);
    }

    public void PreviousLevel()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        currentLevelSelectionIndex--;
        if (currentLevelSelectionIndex < 0)
        {
            currentLevelSelectionIndex += levels.Length;
        }
        view.RPC("UpdateUIRPC", RpcTarget.AllBuffered, currentLevelSelectionIndex);
    }



}
