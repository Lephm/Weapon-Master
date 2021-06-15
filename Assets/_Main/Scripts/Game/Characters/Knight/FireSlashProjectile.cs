using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlashProjectile : ProjectileBase
{
    public GameObject explosionEffect;
    public float explosionRadius = 2.0f;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (IsCollidedWithOwner(collision))
        {
            print("Collided with owners");
            return;
        }

        if (NotCollidedWithPlayers(collision)) return;
        Explode(collision);
    }

    public void Explode(Collider2D collision)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(collision.transform.position, explosionRadius);
        GameObject exploEff = Instantiate(explosionEffect,transform.position,Quaternion.identity);
        BurnEffect burnEffect = exploEff.GetComponent<BurnEffect>();
        burnEffect.BurnPlayers(collisions,owner);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
