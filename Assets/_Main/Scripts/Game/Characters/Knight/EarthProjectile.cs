using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthProjectile : ProjectileBase
{
    [SerializeField] float damage;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (IsCollidedWithOwner(collision)) return;
        CharacterControllerBase enemy = collision.GetComponent<CharacterControllerBase>();
        CharacterControllerBase ownChar = GetOwnerChar();
        if (enemy != null && ownChar != null)
        {
            CharacterControllerBase.ApplyDamage(ownChar, enemy, damage, true);
        }
        
    }
}
