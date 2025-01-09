using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Conectando servidor...");
        PhotonNetwork.ConnectUsingSettings();
    
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Servidor conectado");
        PhotonNetwork.JoinLobby();
    
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Servidor desconectado por " + cause.ToString());
    }

}
