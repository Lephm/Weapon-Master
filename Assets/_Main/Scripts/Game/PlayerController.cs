using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPunCallbacks
{

    CharacterControllerBase characterController;
    PhotonView view;
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
    private void Awake()
    {
        characterController = GetComponent<CharacterControllerBase>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!view.IsMine) return;
        ReceiveKeyboardInput();
    }

    void ReceiveKeyboardInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0 && characterController.moveDirection.x != horizontalInput)
        {
            characterController.Flip(horizontalInput == 1);
        }

        characterController.moveDirection = new Vector2();
        characterController.moveDirection.x = horizontalInput;
        //jump input
        if (Input.GetKeyDown(jumpKey) && characterController.GroundCheck())
        {
            characterController.jump = true;
        }

        if (Input.GetKeyDown(blockKey))
        {
            view.RPC("BlockInputRPC", RpcTarget.AllBuffered);
        }
        if (Input.GetKeyDown(attack1Key))
        {
            view.RPC("Attack1InputRPC", RpcTarget.AllBuffered);

        }
        if (Input.GetKeyDown(attack2Key))
        {
            view.RPC("Attack2InputRPC", RpcTarget.AllBuffered);
        }

        if (Input.GetKeyDown(ultimateAttackKey))
        {
            view.RPC("UltimateAttackInputRPC", RpcTarget.AllBuffered);
        }
    }

}
