using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{

    public GameManager gameManager;

    private void Awake()
    {
        //PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        //PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexión creada");
    }

    public override void OnJoinedRoom()
    {
        GameObject player = PhotonNetwork.Instantiate(GameplayManager.instance.player.name, GameplayManager.instance.roomGenerator.spawnPlayer.position, Quaternion.identity);
      
        //player.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PlayerPrefs.GetString("PlayerName"));
    }
}
