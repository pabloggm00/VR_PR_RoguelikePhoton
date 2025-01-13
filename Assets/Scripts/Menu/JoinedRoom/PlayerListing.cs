using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _namePlayer;

    [SerializeField]
    private Image imagePlayer;

    public GameObject rightButton;
    public GameObject leftButton;


    public PlayerSprites playerSpritesSettings;
    public List<ElementSprite> sprites;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public Player player { get; private set; }

    private void Start()
    {
        sprites = new List<ElementSprite>();

        playerSpritesSettings.AgregarSprites(sprites);
    }

    [PunRPC]
    public void SetPlayerInfo(Player _player)
    {
        this.player = _player;
        _namePlayer.text = _player.NickName;

        UpdatePlayerItem(this.player);
    }

    public void ApplyLocalChanges()
    {
        rightButton.SetActive(true);
        leftButton.SetActive(true);
    }

    public void PreviousElement()
    {

        if ((int)playerProperties["playerSprite"] <= 0)
        {
            playerProperties["playerSprite"] = sprites.Count - 1;
        }
        else
        {
            playerProperties["playerSprite"] = (int)playerProperties["playerSprite"] - 1;
        }


        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        //imagePlayer.sprite = sprites[currentSpriteIndex]; 
    }

    public void NextElement()
    {

        if ((int)playerProperties["playerSprite"] >= sprites.Count - 1)
        {
            playerProperties["playerSprite"] = 0;

        }
        else
        {
            playerProperties["playerSprite"] = (int)playerProperties["playerSprite"] + 1;
        }
        //imagePlayer.sprite = sprites[currentSpriteIndex];

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(player);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerSprite"))
        {
            imagePlayer.sprite = sprites[(int)player.CustomProperties["playerSprite"]].sprite;
            playerProperties["playerSprite"] = (int)player.CustomProperties["playerSprite"];
        }
        else
        {
            playerProperties["playerSprite"] = 0;
        }
    }

   


}


