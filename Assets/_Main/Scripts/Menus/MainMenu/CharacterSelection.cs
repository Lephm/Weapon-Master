using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] charactersDisplay;
    public int selectedCharacter = 0;
    public Text characterNameDisplay;

    public void NextCharacter()
    {
        charactersDisplay[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % charactersDisplay.Length;
        charactersDisplay[selectedCharacter].SetActive(true);
        if(characterNameDisplay != null)
        {
            characterNameDisplay.text = charactersDisplay[selectedCharacter].name;
        }
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }
    
    public void PreviousCharacter()
    {
        charactersDisplay[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter < 0)
        {
            selectedCharacter += charactersDisplay.Length;
        }
        charactersDisplay[selectedCharacter].SetActive(true);
        if (characterNameDisplay != null)
        {
            characterNameDisplay.text = charactersDisplay[selectedCharacter].name;
        }
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        
    }

    
}
