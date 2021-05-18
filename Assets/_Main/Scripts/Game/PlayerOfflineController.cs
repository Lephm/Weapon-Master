using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOfflineController : MonoBehaviour
{
    CharacterControllerBase characterController;

    #region keybinding
    [SerializeField]
    KeyCode jumpKey = KeyCode.W;
    [SerializeField]
    KeyCode blockKey = KeyCode.O;
    [SerializeField]
    KeyCode attack1Key = KeyCode.U;
    [SerializeField]
    KeyCode attack2Key = KeyCode.I;
    [SerializeField]
    KeyCode ultimateAttackKey = KeyCode.R;
    #endregion
    private void Start()
    {
        characterController = GetComponent<CharacterControllerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOffLineInput();
    }

    void UpdateOffLineInput() //for debugging
    {
        if (Input.GetKeyDown(blockKey) && characterController.GroundCheck())
        {
            characterController.Block();
        }
        if (Input.GetKeyDown(attack1Key))
        {
            characterController.Attack1();

        }
        if (Input.GetKeyDown(attack2Key))
        {
            characterController.Attack2();
        }

        if (Input.GetKeyDown(ultimateAttackKey))
        {
            characterController.UltimateAttack();
        }
    }
}
