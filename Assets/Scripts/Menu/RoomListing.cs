using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _text;

    public RoomInfo roomInfo { get; private set; }

    private TMP_Text _playerName;

    public void SetRoomInfo(RoomInfo room, TMP_Text playerName)
    {
        _playerName = playerName;
        roomInfo = room;
        _text.text = room.MaxPlayers + ",  " + room.Name;
    }

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.NickName = _playerName.text;
        Debug.Log("Bienvenido a la sala " + PhotonNetwork.NickName);
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
}
