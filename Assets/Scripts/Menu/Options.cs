using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Options : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public GameObject menuPanel; // El panel del menú de opciones
    public GameObject resumeButton; // Botón para reanudar
    public GameObject exitButton;   // Botón para salir

    private bool isPaused = false;

    public override void OnEnable()
    {
        InputManager.playerControls.UI.Pausa.performed += OnPause;
        InputManager.playerControls.UI.Pausa.canceled += OnPause;
    }

    public override void OnDisable()
    {
        InputManager.playerControls.UI.Pausa.performed -= OnPause;
        InputManager.playerControls.UI.Pausa.canceled -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && photonView.IsMine)
        {
            ToggleMenu();
        }
    }

    private void Start()
    {
        menuPanel.SetActive(false); // Asegurarse de que el menú esté oculto al inicio
    }

    private void ToggleMenu()
    {
        isPaused = !isPaused;
        menuPanel.SetActive(isPaused); // Activar o desactivar el menú localmente
    }

    public void OnResumeButtonClicked()
    {
        if (photonView.IsMine)
        {
            ToggleMenu(); // Cerrar el menú solo localmente
        }
    }

    public void OnExitButtonClicked()
    {
        if (photonView.IsMine)
        {
            exitButton.GetComponent<Button>().interactable = false;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Menu"); // Volver al menú principal
        }
    }

   
}
