using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject[] charactersPrefabs;
    public Transform[] spawnPoints;
    public delegate void WinnerFound(CharacterControllerBase winner);
    public static event WinnerFound WinnerFoundEvent;
    public const byte NetworkWinnerFoundEventCode = 1;
    public Collider2D boundaries;
    public GameObject mainCine2DCamera;
    #region UI
    public GameObject characterDisplayHolders;
    public GameObject characterDisplayUI;
    public GameObject diePanel;
    public GameObject endGamePanel;
    public GameObject restartButton;
    public Text winnerAnnounceText;
    #endregion
    [Header("Audio")]
    public AudioSource m_audio;
    public AudioClip backgroundMusic;

    private void Awake()
    {
        SpawnPlayers();
        WinnerFoundEvent += OnWinnerFound;
        PhotonNetwork.AutomaticallySyncScene = true;
        m_audio = GetComponent<AudioSource>();

    }
    public virtual void OnEnable()
    {
        base.OnEnable();
        Health.OnCharacterDieEvent += OnPlayerDie;
        PhotonNetwork.AddCallbackTarget(this); 
    }

    public virtual void OnDisable()
    {
        base.OnDisable();
        Health.OnCharacterDieEvent -= OnPlayerDie;
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void Start()
    {   
        // invoke after 1.5 secs to make sure all players is instatiated
        Invoke("SetPlayerDisplayNames", 1.5f);
        Invoke("SetupCine2DCamera", 1.5f);
        //Close room when the game start
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // makes room close 
            PhotonNetwork.CurrentRoom.IsVisible = false; // makes room invisible to random match
        }

        if(PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating("AttempToFindWinner", 10.0f, 2.0f);
        }

        //Play background music
        if(backgroundMusic != null)
        {
            m_audio.clip = backgroundMusic;
            m_audio.Play();
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


    private void OnPlayerDie(Health player)
    {   
        if(player.view.IsMine)
        {
            diePanel.SetActive(true);
            PlayerController playerCon = player.GetComponent<PlayerController>();
            if(playerCon != null)
            {
                playerCon.enabled = false;
            }

        }
        
    }

    private void OnWinnerFound(CharacterControllerBase winner)
    {
        diePanel.SetActive(false);
        endGamePanel.SetActive(true);
        if(winner != null)
        {
            winnerAnnounceText.text = "Winner is " + winner.view.Owner.NickName;
        }

        else
        {
            winnerAnnounceText.text = "Draw";
        }
        
        if(PhotonNetwork.IsMasterClient)
        {
            winnerAnnounceText.text += "\n PLease Restart the game";
            restartButton.gameObject.SetActive(true);
        }

        else
        {
            winnerAnnounceText.text += "\n PLease Wait For The Host to Restart";
            restartButton.gameObject.SetActive(false);
        }
    }

    public void AttempToFindWinner()
    {
        //Attemp to find winner
        if (PhotonNetwork.IsMasterClient)
        {
            CharacterControllerBase[] characters = FindObjectsOfType<CharacterControllerBase>(false);
            //Only 1 player alive
            if (characters.Length == 1)
            {
                CharacterControllerBase character = characters[0];
                int winnerViewID = 12323211; // this is just a random number for place holder
                if (character != null)
                {
                    winnerViewID = character.view.ViewID;
                    NetworkFoundWinnerEventTrigger(winnerViewID);
                }
                
                else
                {
                    NetworkFoundWinnerEventTrigger(winnerViewID);
                }           
            }

        }
    }

    //Only call on Master Client
    public void NetworkFoundWinnerEventTrigger(int viewID)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkWinnerFoundEventCode, viewID, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == NetworkWinnerFoundEventCode)
        {
            int winnerID = (int)photonEvent.CustomData;
            print(winnerID);
            PhotonView winner = PhotonView.Find(winnerID);
            CharacterControllerBase winnerChar = null;
            if (winner == null)
            {
                if (WinnerFoundEvent != null)
                {
                    WinnerFoundEvent.Invoke(null);
                }
                
            }
            else
            {
                winnerChar = winner.GetComponent<CharacterControllerBase>();
                if (winnerChar == null)
                {
                    if (WinnerFoundEvent != null)
                    {
                        WinnerFoundEvent.Invoke(null);
                    }
                }
                else
                {
                    WinnerFoundEvent.Invoke(winnerChar);
                }
            }
            
        }
    }

    public void RestartGame()
    {
        PhotonNetwork.LoadLevel("Room");
    }
    public void SetupCine2DCamera()
    {
        if (mainCine2DCamera == null) return;

        if (boundaries == null)
        {
            GameObject bound = GameObject.Find("Boundaries");
            if(bound != null)
            {
                boundaries = bound.GetComponent<Collider2D>();
            }  
        }

        if (boundaries != null)
        {
            CinemachineConfiner cinemachineConfiner = mainCine2DCamera.GetComponent<CinemachineConfiner>();
            if (cinemachineConfiner != null)
            {
                cinemachineConfiner.m_BoundingShape2D = boundaries;
            }
        }

        CharacterControllerBase[] characters = FindObjectsOfType<CharacterControllerBase>(false);
        CinemachineVirtualCamera cinemachine = mainCine2DCamera.GetComponent<CinemachineVirtualCamera>();
        foreach (CharacterControllerBase character in characters)
        {
            PhotonView view = character.GetComponent<PhotonView>();
            if (view != null && cinemachine != null)
            {
                if (view.IsMine)
                {
                    cinemachine.Follow = view.transform;
                    break;
                }
            }
        }


    }   
}
