using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public Text characterNameDisplay;

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        if(characterNameDisplay != null)
        {
            characterNameDisplay.text = characters[selectedCharacter].name;
        }
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }
    
    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
        if (characterNameDisplay != null)
        {
            characterNameDisplay.text = characters[selectedCharacter].name;
        }
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }

    
}
