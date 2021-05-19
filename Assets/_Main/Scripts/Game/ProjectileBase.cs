using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ProjectileBase : MonoBehaviourPunCallbacks
{
    [SerializeField] float speed = 3.0f;
    public bool moveRight = true;
    private GameObject owner;

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
    }
    private void Awake()
    {

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

    public bool IsCollidedWithLocalPlayer(Collider2D collision)
    {
        return owner == collision.gameObject;
    }
    public virtual void OnDestroy()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {   
      
    }

}
