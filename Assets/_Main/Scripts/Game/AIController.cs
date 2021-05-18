using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    CharacterControllerBase characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterControllerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Block();
    }
}
