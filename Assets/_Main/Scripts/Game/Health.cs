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
    bool isDead = false;
    PhotonView view;
    private void Awake()
    {
        currentHealth = maxHealth;
        view = GetComponent<PhotonView>();
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

        if(PhotonNetwork.IsMasterClient)
        {
            view.RPC("ResetToServerHealth", RpcTarget.AllBuffered, currentHealth);
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

    [PunRPC]
    public void ResetToServerHealth(float currentCharHealth)
    {   
        if(GetIsDead() == false)
        {
            currentHealth = currentCharHealth;
        }

        //make sure the character die on all client
        else
        {
            TakeDamage(currentHealth);
        }
        
    }

    public bool GetIsDead()
    {
        return isDead;
    }
    
}
