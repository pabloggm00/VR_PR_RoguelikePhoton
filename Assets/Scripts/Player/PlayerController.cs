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

    private void Start()
    {
        Init();

        foreach (ElementSprite element in spritesSettings.elementSprites)
        {
            elementSouls.Add(element.elementType, 0);
        }

        AddSoul(ElementType.Agua, 2);
    }

    public void Init()
    {
        elementSpritePlayer = new List<ElementSprite>();

        spritesSettings.AgregarSprites(elementSpritePlayer);

        elementCurrent = elementSpritePlayer[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerSprite"]];
        spriteRenderer.sprite = elementCurrent.sprite;

    }

    public void ChangeElement(ElementSprite tipoACambiar)
    {
        elementCurrent = tipoACambiar;
    }

    public void AddSoul(ElementType tipo, int cantidad)
    {
        elementSouls[tipo] += cantidad;
    }

}
