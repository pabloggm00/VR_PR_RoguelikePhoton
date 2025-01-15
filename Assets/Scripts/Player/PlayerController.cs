using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{

    public SpriteRenderer spriteRenderer;
    public PlayerSprites spritesSettings;

    public ElementSprite tipo;
    List<ElementSprite> elementSpritePlayer;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        elementSpritePlayer = new List<ElementSprite>();

        spritesSettings.AgregarSprites(elementSpritePlayer);

        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["playerSprite"]);
        tipo = elementSpritePlayer[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerSprite"]];
        spriteRenderer.sprite = tipo.sprite;

    }

    public void ChangeElement(ElementSprite tipoACambiar)
    {
        tipo = tipoACambiar;
    }
}
