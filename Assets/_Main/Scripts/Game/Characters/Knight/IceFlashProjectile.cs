using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFlashProjectile : ProjectileBase
{
    [SerializeField] GameObject frozenEffect;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (IsCollidedWithOwner(collision))
        {
            print("Collided with owners");
            return; 
        } // can make an rpc function and only call on master client if there are too many bugs with it

        if (NotCollidedWithPlayers(collision)) return;

        CharacterControllerBase controllerBase = collision.GetComponent<CharacterControllerBase>();
        if(controllerBase != null)
        {
            if(controllerBase.GetCurrentlyIsBlocking())
            {
                Destroy(this.gameObject);
            }
        }

        PlayerController playerController = collision.GetComponent<PlayerController>();
        if(playerController != null)
        {
            playerController.enabled = false;
            controllerBase.moveDirection = Vector2.zero;
            GameObject frozeParticle = Instantiate(frozenEffect, playerController.transform.position, Quaternion.identity);
            FrozenEffect frozenEff = frozeParticle.GetComponent<FrozenEffect>();
            frozenEff.EnablePlayerController(playerController);
        }

        Destroy(this.gameObject);
        
    }

}

