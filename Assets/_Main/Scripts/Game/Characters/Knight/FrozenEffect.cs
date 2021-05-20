using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenEffect : MonoBehaviour
{
    public float frozeTime = 2.0f;

    public void EnablePlayerController(PlayerController player)
    {
        StartCoroutine("EnablePlayerControllerRoutine", player);
    }
   IEnumerator EnablePlayerControllerRoutine(PlayerController player)
   {
        yield return new WaitForSeconds(frozeTime);
        player.enabled = true;
        Destroy(this.gameObject);
   }
}
