using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
public class StartupScript : MonoBehaviourPunCallbacks
{
    [SerializeField]TextMeshProUGUI loadingText;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject selectRegionPanel;
    public string[] regionsCodes;
    int selectedRegionIndex = 0;
    public TextMeshProUGUI regionCodeDisplay;
    private string selectedRegionCode;
    bool isLoading = true;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedRegionCode = regionsCodes[0];
    }

    public void StartGame()
    {
        isLoading = true;
        loadingPanel.SetActive(false);
        loadingPanel.SetActive(true);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = selectedRegionCode;
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

    

    public void NextRegion()
    {
        selectedRegionIndex = (selectedRegionIndex + 1) % regionsCodes.Length;
        selectedRegionCode = regionsCodes[selectedRegionIndex];
        regionCodeDisplay.text = selectedRegionCode;
        
    }


    
    






}
