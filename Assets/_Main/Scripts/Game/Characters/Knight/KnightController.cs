using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : CharacterControllerBase
{
    [Header("Knight Combat Stats")]
    [SerializeField]Transform attackPos;
    [SerializeField] float attackRadius;
    [SerializeField] GameObject combo1Projectile;
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
        Debug.LogWarning("Excute Combo 1");
        SpawnProjectile(combo1Projectile);
    }

    public override void SetupComboDictionary()
    {
        base.SetupComboDictionary();
        comboDictionary.Add(Combo1Condition, Combo1Attack);
    }




}
