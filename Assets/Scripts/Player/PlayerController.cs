using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public PlayerSprites spritesSettings;

    ElementType tipo;
    List<Sprite> sprites;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        sprites = new List<Sprite>();

        spritesSettings.AgregarSprites(sprites);

        spriteRenderer.sprite = sprites[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerSprite"]];
        
        
    }
}
