using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSlashProjectile : ProjectileBase
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (IsCollidedWithOwner(collision)) return;
    }
}
