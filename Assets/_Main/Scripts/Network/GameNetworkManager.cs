using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject[] charactersPrefabs;
    public Transform[] spawnPoints;
    public CharacterData[] charDatas;

    #region UI
    public GameObject characterDisplayHolders;
    public GameObject characterDisplayUI;
    #endregion

    #region UI

    #endregion

    private void Awake()
    {
        SpawnPlayers();
    }

    private void Start()
    {   
        // invoke after 1.5 secs to make sure all players is instatiated
        Invoke("SetPlayerDisplayNames", 1.5f);
    }

    public void SetPlayerDisplayNames()
    {
        //Set players' displayName
        CharacterControllerBase[] characters = FindObjectsOfType<CharacterControllerBase>(false);
        foreach (CharacterControllerBase character in characters)
        {
            PhotonView view = character.GetComponent<PhotonView>();
            if (view != null && character.nameDisplay != null)
            {
                character.nameDisplay.text = view.Owner.NickName;
                if (view.IsMine)
                {
                    character.nameDisplay.color = Color.green;
                }

                else
                {
                    character.nameDisplay.color = Color.red;
                }

            }

            int charIndex = character.characterClassIndex;
            if (charIndex >= charDatas.Length)
            {
                charIndex = 0;
            }

            CreateANewCharacterDisplay(charDatas[charIndex], character);
        }
    }

    public void CreateANewCharacterDisplay(CharacterData data,CharacterControllerBase character)
    {
        GameObject characterDisplay = Instantiate(characterDisplayUI, characterDisplayHolders.transform);
        CharacterStatsDisplay displayStat = characterDisplay.GetComponent<CharacterStatsDisplay>();
        if(displayStat != null)
        {
            displayStat.Setup(data, character);
        }
    }

    private void SpawnPlayers()
    {
        int randomInt = Random.Range(0, spawnPoints.Length - 1);
        int selectedIndex = PlayerPrefs.GetInt("selectedIndex", 0);
        GameObject character = PhotonNetwork.Instantiate(charactersPrefabs[selectedIndex].name, spawnPoints[randomInt].position, Quaternion.identity);
        CharacterControllerBase player = character.GetComponent<CharacterControllerBase>();
    }

    private void SpawnAI()
    {   
        if(PhotonNetwork.IsMasterClient)
        {
            int randomInt = Random.Range(0, spawnPoints.Length - 1);
            int selectedIndex = PlayerPrefs.GetInt("selectedIndex", 0);
            GameObject character = PhotonNetwork.Instantiate(charactersPrefabs[selectedIndex].name, spawnPoints[randomInt].position, Quaternion.identity);
            character.GetComponent<PlayerController>().enabled = false;
            character.AddComponent<AIController>();
            CharacterControllerBase aiChar = character.GetComponent<CharacterControllerBase>();
            if(aiChar != null && aiChar.nameDisplay != null)
            {
                aiChar.nameDisplay.text = "AI";
                aiChar.nameDisplay.color = Color.red;
            }

        }
        
    }
}
