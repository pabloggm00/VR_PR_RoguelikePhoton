using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrearSala : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TMP_Text playerName;

    [SerializeField]
    private TMP_Text roomName;

    public void OnClick_CreateRoom()
    {

        if (!PhotonNetwork.IsConnected || playerName.text.Length == 0)
            return;
        PhotonNetwork.NickName = playerName.text;
       
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room creada");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room no creada por " + message);
    }
}
