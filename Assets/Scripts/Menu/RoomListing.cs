using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomListing : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TMP_Text _roomName;

    [SerializeField]
    private TMP_Text _roomNumPlayers;

    public RoomInfo roomInfo { get; private set; }

    private TMP_Text _playerName;

    public void SetRoomInfo(RoomInfo room, TMP_Text playerName)
    {
        _playerName = playerName;
        roomInfo = room;
        _roomName.text = room.Name;
        _roomNumPlayers.text = room.PlayerCount + "/" + room.MaxPlayers;
    }

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.NickName = _playerName.text;
        Debug.Log("Bienvenido a la sala " + PhotonNetwork.NickName);
        PhotonNetwork.JoinRoom(roomInfo.Name);
        
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
