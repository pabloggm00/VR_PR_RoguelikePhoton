using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{

    public SpriteRenderer spriteRenderer;
    public PlayerSprites spritesSettings;

    
    public ElementSprite elementCurrent {  get; private set; }
    List<ElementSprite> elementSpritePlayer;

    private Dictionary<ElementType, int> elementSouls = new Dictionary<ElementType, int>();
    int maxSouls;
    public static event Action<ElementType, int> UpdateSoulHUD;


    public override void OnEnable()
    {
        // Suscribirse a las acciones del grupo ChangeElement
        InputManager.playerControls.Player.ChangeElement1.performed += ctx => OnChangeElement(0);
        InputManager.playerControls.Player.ChangeElement2.performed += ctx => OnChangeElement(1);
        InputManager.playerControls.Player.ChangeElement3.performed += ctx => OnChangeElement(2);
        InputManager.playerControls.Player.ChangeElement4.performed += ctx => OnChangeElement(3);
    }

    public override void OnDisable()
    {
        InputManager.playerControls.Player.ChangeElement1.performed -= ctx => OnChangeElement(0);
        InputManager.playerControls.Player.ChangeElement2.performed -= ctx => OnChangeElement(1);
        InputManager.playerControls.Player.ChangeElement3.performed -= ctx => OnChangeElement(2);
        InputManager.playerControls.Player.ChangeElement4.performed -= ctx => OnChangeElement(3);
    }

    private void OnChangeElement(int elementIndex)
    {
        if (!photonView.IsMine) return;

        if (elementIndex >= 0 && elementIndex < elementSpritePlayer.Count)
        {
            photonView.RPC("ChangeElementRPC", RpcTarget.All, elementIndex);
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            int selectedSpriteIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterIndex"];
            photonView.RPC("Init", RpcTarget.All, selectedSpriteIndex);
        }
    }

    [PunRPC]
    public void ChangeElementRPC(int elementIndex)
    {
        elementCurrent = elementSpritePlayer[elementIndex];
        spriteRenderer.sprite = elementCurrent.sprite;
    }

    [PunRPC]
    public void Init(int spriteIndex)
    {
        elementSpritePlayer = new List<ElementSprite>(); //creo la lista de ls sprites

        spritesSettings.AgregarSprites(elementSpritePlayer); //agrego los sprites

        elementCurrent = elementSpritePlayer[spriteIndex]; //seteo el tipo

        spriteRenderer.sprite = elementCurrent.sprite; //seteo el spite

        maxSouls = GameplayManager.instance.soulsNeeded; //cantidad máxima de recolectar souls

        foreach (ElementSprite element in spritesSettings.elementSprites)
        {
            elementSouls.Add(element.elementType, 0); //agrego el diccionario los distintos souls con su cantidad inicial
        }

    }

    public void ChangeElement(ElementSprite tipoACambiar)
    {
        elementCurrent = tipoACambiar;
        spriteRenderer.sprite = tipoACambiar.sprite;
    }

    public void AddSoul(ElementType tipo, int cantidad)
    {
        elementSouls[tipo] += cantidad;

        if (elementSouls[tipo] > maxSouls)
        {
            elementSouls[tipo] = maxSouls;
        }

        UpdateSoulHUD?.Invoke(tipo, cantidad);
    }

}
