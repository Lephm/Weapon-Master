using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ProjectileBase : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 3.0f;
    public bool moveRight = true;
    public GameObject owner;
    public GameObject destroyEffect;
    public float lifeTime = 10.0f;
    public SpriteRenderer projSprite;
    public virtual void OnEnable()
    {
        Destroy(this.gameObject,lifeTime);
    }
    public void SetupProjectile(float spawnerYRotation, GameObject spawner)
    {
        if (spawnerYRotation == -180 || spawnerYRotation == 180)
        {
            moveRight = false;
        }

        else
        {
            moveRight = true;
        }

        owner = spawner;

        if (projSprite != null)
        {
            projSprite.flipX = !moveRight;
        }

        
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(moveRight)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }

        else
        {
            transform.Translate(-1*transform.right * speed * Time.deltaTime);
        }
        
    }

    public virtual void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }

    public bool IsCollidedWithOwner(Collider2D collision)
    {
        return owner == collision.gameObject;
    }
    public virtual void OnDestroy()
    {
        if(destroyEffect != null)
        {
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
        }
    }

    public bool NotCollidedWithPlayers(Collider2D collision)
    {
        CharacterControllerBase character = collision.GetComponent<CharacterControllerBase>();
        return character == null;
    }

    public bool CollidedPlayerIsBlocking(Collider2D collision)
    {
        CharacterControllerBase controllerBase = collision.GetComponent<CharacterControllerBase>();
        if(controllerBase == null)
        {
            return false;
        }

        else
        {
            return controllerBase.GetCurrentlyIsBlocking();
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {   

    }

}
