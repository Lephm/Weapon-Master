using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Health : MonoBehaviourPunCallbacks
{
    public delegate void OnCharacterDie(Health character);
    public static OnCharacterDie OnCharacterDieEvent;
    [SerializeField] float maxHealth = 100.0f;
    float currentHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        print(currentHealth);
        if(currentHealth <= 0)
        {
            print("Die");
            OnCharacterDieEvent.Invoke(this);
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    
}
