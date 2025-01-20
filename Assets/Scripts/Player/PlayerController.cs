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

    public bool canChangeToWater = false;
    public bool canChangeToFire = false;
    public bool canChangeToLeaf = false;
    public bool canChangeToRock = false;

    public override void OnEnable()
    {
        // Suscribirse a las acciones del grupo ChangeElement
        InputManager.playerControls.Player.ChangeElement1.performed += ctx => OnChangeElement(0, canChangeToWater);
        InputManager.playerControls.Player.ChangeElement2.performed += ctx => OnChangeElement(1, canChangeToFire);
        InputManager.playerControls.Player.ChangeElement3.performed += ctx => OnChangeElement(2, canChangeToLeaf);
        InputManager.playerControls.Player.ChangeElement4.performed += ctx => OnChangeElement(3, canChangeToRock);
    }

    public override void OnDisable()
    {
        InputManager.playerControls.Player.ChangeElement1.performed -= ctx => OnChangeElement(0, canChangeToWater);
        InputManager.playerControls.Player.ChangeElement2.performed -= ctx => OnChangeElement(1, canChangeToFire);
        InputManager.playerControls.Player.ChangeElement3.performed -= ctx => OnChangeElement(2, canChangeToLeaf);
        InputManager.playerControls.Player.ChangeElement4.performed -= ctx => OnChangeElement(3, canChangeToRock);
    }

    private void OnChangeElement(int elementIndex, bool canChange)
    {
        if (!photonView.IsMine) return; // Solo el propietario puede cambiar de elemento

        if (canChange && elementIndex >= 0 && elementIndex < elementSpritePlayer.Count)
        {
            photonView.RPC("ChangeElementRPC", RpcTarget.All, elementIndex);
            ResetElementState(elementIndex);
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
    private void ResetElementState(int elementIndex)
    {
        ElementType type = elementSpritePlayer[elementIndex].elementType;

        // Reiniciar souls y booleano
        elementSouls[type] = 0;
        UpdateSoulHUD?.Invoke(type, 0); // Actualizar la interfaz gráfica

        switch (type)
        {
            case ElementType.Agua:
                canChangeToWater = false;
                break;
            case ElementType.Fuego:
                canChangeToFire = false;
                break;
            case ElementType.Hoja:
                canChangeToLeaf = false;
                break;
            case ElementType.Piedra:
                canChangeToRock = false;
                break;
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

        CheckElementUnlock(tipo);

        UpdateSoulHUD?.Invoke(tipo, elementSouls[tipo]);
    }

    private void CheckElementUnlock(ElementType tipo)
    {
        if (elementSouls[tipo] == maxSouls)
        {
            switch (tipo)
            {
                case ElementType.Agua:
                    canChangeToWater = true;
                    break;
                case ElementType.Fuego:
                    canChangeToFire = true;
                    break;
                case ElementType.Hoja:
                    canChangeToLeaf = true;
                    break;
                case ElementType.Piedra:
                    canChangeToRock = true;
                    break;
            }
        }
    }

}
