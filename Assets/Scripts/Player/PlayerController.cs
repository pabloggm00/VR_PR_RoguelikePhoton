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

    private void Start()
    {
        if (photonView.IsMine)
        {
            int selectedSpriteIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterIndex"];
            photonView.RPC("Init", RpcTarget.AllBuffered, selectedSpriteIndex);
        }
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
