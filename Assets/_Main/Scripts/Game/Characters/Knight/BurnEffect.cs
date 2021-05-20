using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    public float animationWaitTime = 0.25f;
    public float burnTimeBetweenEach = 0.25f;
    public float burnDamage = 2.0f;
    public int burnTimes = 4;
    public SpriteRenderer[] rends;


    public void BurnPlayers(Collider2D[] collisions, GameObject owner)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("collisions", collisions);
        dict.Add("owner", owner);
        StartCoroutine("BurnPlayerRoutine", dict);
    }

    IEnumerator BurnPlayerRoutine(Dictionary<string, object> dict)
    {
        Collider2D[] collisions = (Collider2D[])dict["collisions"];
        GameObject owner = (GameObject)dict["owner"];
        int i = 0;
        yield return new WaitForSeconds(animationWaitTime);
        foreach(SpriteRenderer rend in rends)
        {
            rend.enabled = false;
        }
        while(i < burnTimes)
        {
            i++;
            foreach(Collider2D collider in collisions)
            {   
                if(collider == null)
                {
                    continue;
                }

                if(collider.gameObject == owner)
                {
                    continue;
                }
                Health health = collider.GetComponent<Health>();
                if(health != null)
                {
                    health.TakeDamage(burnDamage);
                }
            }

            yield return new WaitForSeconds(burnTimeBetweenEach);
        }

        Destroy(this.gameObject);

    }
    
}
