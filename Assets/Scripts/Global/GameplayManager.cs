using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    //public RoomGenerator roomGenerator;
    public GameObject playerPrefab;
    public Transform spawnPoint;
    //public GameObject poolParent;
    public int soulsNeeded = 5;

    public List<GameObject> playersInGame = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
        //InitWorld();
    }

    private void SpawnPlayer()
    {
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);

        // Configurar el jugador local
        PlayerSetup playerSetup = playerObject.GetComponent<PlayerSetup>();
        if (playerSetup != null && PhotonNetwork.LocalPlayer.IsLocal)
        {
            playerSetup.IsLocalPlayer();
            playerObject.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }

        AgregarJugador(playerObject);
    }

    void InitWorld()
    {
        // roomGenerator.GenerateRoom();
        //roomGenerator.InitGame(player);
    }

    public void AgregarJugador(GameObject player)
    {
        playersInGame.Add(player);
    }

}
