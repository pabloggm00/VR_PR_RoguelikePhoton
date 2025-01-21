using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fin : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.playerControls.UI.Fin.performed -= FinInput;
        InputManager.playerControls.UI.Fin.canceled -= FinInput;
    }

    private void OnDisable()
    {
        InputManager.playerControls.UI.Fin.performed -= FinInput;
        InputManager.playerControls.UI.Fin.canceled -= FinInput;
    }

    private void FinInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Menu");
        }
    }
}
