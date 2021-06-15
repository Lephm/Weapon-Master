using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStatsDisplay : MonoBehaviour
{
    public Image characterAvatar;
    public Image healthBar;
    public Image blockProgressBar;
    public CharacterControllerBase character;
    public Health characterHealth;
    public Image ultimateMeterProgressBar;


    public void Setup(CharacterControllerBase charController)
    {
        if (charController.characterAvatar != null)
        {
            characterAvatar.sprite = charController.characterAvatar;
        }
        character = charController;
        characterHealth = character.GetComponent<Health>();
    }
    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (character == null || characterHealth == null)
        {   
            return;
        }
        healthBar.fillAmount = characterHealth.GetCurrentHealth() / characterHealth.GetMaxHealth();
        float displayBlockProgressTime = character.GetTimeSinceLastBlock();
        if(displayBlockProgressTime > character.GetBlockCooldown())
        {
            displayBlockProgressTime = character.GetBlockCooldown();
        }

        blockProgressBar.fillAmount =  displayBlockProgressTime/ character.GetBlockCooldown();
        float displayUltimateProgressTime = character.GetCurrentUltimateMeter();
        if (displayUltimateProgressTime > character.GetMaxUltimateMeter())
        {
            displayUltimateProgressTime = character.GetMaxUltimateMeter();
        }
        ultimateMeterProgressBar.fillAmount = displayUltimateProgressTime / character.GetMaxUltimateMeter();
    }

    public void OnCharacterOfThisDisplayDie()
    {

    }
}
