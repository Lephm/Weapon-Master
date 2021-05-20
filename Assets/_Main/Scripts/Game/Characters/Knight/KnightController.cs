using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KnightController : CharacterControllerBase
{
    [Header("Knight Combat Stats")]
    [SerializeField]Transform attackPos;
    [SerializeField] float attackRadius;
    [SerializeField] GameObject iceProjectile;
    [SerializeField] GameObject fireProjectile;
    [SerializeField] GameObject earthProjectile;
    int randIntForCombo1 = 1;
    public override void Awake()
    {
        base.Awake();
        if(attackPos is null)
        {
            attackPos = this.gameObject.GetComponentInChildren<Transform>();
        }    
            
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }

    public override void OnAttack1Hit()
    {
        base.OnAttack1Hit();
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPos.position, attackRadius);
        foreach (Collider2D Hit in Hits)
        {
            if (Hit.Equals(this.GetComponent<Collider2D>())) continue; 
            CharacterControllerBase Enemy = Hit.GetComponent<CharacterControllerBase>();
            if(Enemy != null)
            {
                CharacterControllerBase.ApplyDamage(this, Enemy, attack1Damage, false);
            }
        }

    }

    public override void OnAttack2Hit()
    {
        base.OnAttack2Hit();
        Collider2D[] Hits = Physics2D.OverlapCircleAll(attackPos.position, attackRadius);
        foreach (Collider2D Hit in Hits)
        {
            if (Hit.Equals(this.GetComponent<Collider2D>())) continue;
            CharacterControllerBase Enemy = Hit.GetComponent<CharacterControllerBase>();
            if (Enemy != null)
            {
                CharacterControllerBase.ApplyDamage(this, Enemy, attack2Damage, false);
            }
        }
    }

    public bool Combo1Condition(List<int> attackInputSchedule)
    {
        List<int> condition = new List<int>();
        condition.Add(1);
        condition.Add(1);
        condition.Add(2);
        return CheckAComboCondition(condition);
    }

    public void Combo1Attack()
    {  
        if(PhotonNetwork.IsMasterClient)
        {
            int randomInt = Random.Range(1, 4);
            view.RPC("Combo1AttackRPC", RpcTarget.AllBuffered, randomInt);
        }
    }

    [PunRPC]
    public void Combo1AttackRPC(int randint)
    {
        randIntForCombo1 = randint;
        Debug.LogWarning("Excute Combo 1");
        anim.SetTrigger("combo1Anim");

    }

    public override void SetupComboDictionary()
    {
        base.SetupComboDictionary();
        //Elemental Slash
        comboDictionary.Add(Combo1Condition, Combo1Attack);
    }

    #region combo Animation Event

    public void OnCombo1AnimationPlayed()
    {
        
        switch (randIntForCombo1)
        {   

            case 1:
                SpawnProjectile(iceProjectile);
                break;
            case 2:
                SpawnProjectile(fireProjectile);
                break;
            case 3:
                SpawnProjectile(earthProjectile);
                break;
            default:
                break;
        }
    }

    #endregion



}
