using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    public Player player {  get; private set; }

    public void SetPlayerInfo(Player player)
    {
        this.player = player;
        _text.text = player.NickName;
    }
}
