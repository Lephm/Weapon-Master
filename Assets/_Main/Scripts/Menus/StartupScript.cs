using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
public class StartupScript : MonoBehaviourPunCallbacks
{
    [SerializeField]TextMeshProUGUI loadingText;
    bool isLoading = true;
    
    // Start is called before the first frame update
    void Start()
    {   
        isLoading = true;
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine("LoadingTextAnimation");
        
    }

    IEnumerator LoadingTextAnimation()
    {
        if (loadingText == null) yield return null;
        while (isLoading)
        {
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.25f);
            loadingText.text = "Loading....";
            yield return new WaitForSeconds(0.25f);

        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("Connected to a Lobby");
        isLoading = false;
        SceneManager.LoadScene("MainMenu");
    }





}
