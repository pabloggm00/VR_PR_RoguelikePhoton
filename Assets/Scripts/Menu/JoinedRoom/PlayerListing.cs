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

    public List<Sprite> sprites;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public Player player {  get; private set; }

    [PunRPC]
    public void SetPlayerInfo(Player player)
    {
        this.player = player;
        _namePlayer.text = player.NickName;

        imagePlayer.sprite = sprites[0];
    }

    public void PreviousElement()
    {

        if ((int)playerProperties["playerSprite"] < 0)
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

        if ((int)playerProperties["playerSprite"] >= sprites.Count)
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
            UpdatePlayerItem();
        }
    }

    void UpdatePlayerItem()
    {
        if (player.CustomProperties.ContainsKey("playerSprite"))
        {
            imagePlayer.sprite = sprites[(int)playerProperties["playerSprite"]];
            playerProperties["playerSprite"] = (int)player.CustomProperties["playerSprite"];
        }
        else
        {
            playerProperties["playerSprite"] = 0;
        }
    }
}
