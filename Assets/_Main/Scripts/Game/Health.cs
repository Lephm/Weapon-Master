using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Health : MonoBehaviourPunCallbacks
{
    public delegate void OnCharacterDie(Health character);
    public static OnCharacterDie OnCharacterDieEvent;
    Animator anim;
    [SerializeField] float maxHealth = 100.0f;
    float currentHealth;
    public PhotonView view;
    bool hasPlayedDeathAnimation = false;
    private void Awake()
    {
        currentHealth = maxHealth;
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        //Only apply damage on master client
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
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
    public void TakeDamageRPC(float damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("takeDamage");
        print("take Damage");
        print(currentHealth);
        if (currentHealth <= 0)
        {
            print("Die");
            //TODO: Spawn Particle Effect
            if (OnCharacterDieEvent != null)
            {
                OnCharacterDieEvent.Invoke(this);
            }
            //hasDied is to check for animation that transits from any states
            anim.SetBool("hasDied", true);
            if (hasPlayedDeathAnimation == false)
            {
                hasPlayedDeathAnimation = true;
                anim.SetTrigger("die");
            }

            Destroy(this.gameObject, 5.0f);
        }
    }


    
}
