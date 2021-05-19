using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] charactersPrefabs;
    public Transform[] spawnPoints;
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
        //Close room when the game start
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // makes room close 
            PhotonNetwork.CurrentRoom.IsVisible = false; // makes room invisible to random match
        }
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

            CreateANewCharacterDisplay(character);
        }
    }

    public void CreateANewCharacterDisplay(CharacterControllerBase character)
    {
        GameObject characterDisplay = Instantiate(characterDisplayUI, characterDisplayHolders.transform);
        CharacterStatsDisplay displayStat = characterDisplay.GetComponent<CharacterStatsDisplay>();
        if(displayStat != null)
        {
            displayStat.Setup(character);
        }
    }

    private void SpawnPlayers()
    {
        int randomInt = Random.Range(0, spawnPoints.Length - 1);
        int selectedIndex = PlayerPrefs.GetInt("selectedIndex", 0);
        GameObject character = PhotonNetwork.Instantiate(charactersPrefabs[selectedIndex].name, spawnPoints[randomInt].position, Quaternion.identity);
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        StartCoroutine("DestroyLeftOverUI");  
        
    }

    //Destroy character ui when that character leave game 
    IEnumerator DestroyLeftOverUI()
    {   
        //make sure that the player leave and every game get updated
        yield return new WaitForSeconds(1.5f);
        CharacterStatsDisplay[] statsDisplays = FindObjectsOfType<CharacterStatsDisplay>(false);
        foreach (CharacterStatsDisplay statsDisplay in statsDisplays)
        {
            print(statsDisplay.character);
            if (statsDisplay.character == null)
            {
                Destroy(statsDisplay.gameObject);
            }
        }
    }
}
