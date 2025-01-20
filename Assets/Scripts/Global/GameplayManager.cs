using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    //public RoomGenerator roomGenerator;
    public GameObject playerPrefab;
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;
    //public GameObject poolParent;
    public int soulsNeeded = 10;

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
        Transform spawnPoint = GetSpawnPoint(PhotonNetwork.LocalPlayer);
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

    private Transform GetSpawnPoint(Photon.Realtime.Player player )
    {
        if (player.IsMasterClient)
        {
            return spawnPointPlayer1;
        }

        return spawnPointPlayer2;
    }

    public Transform FindNearestPlayer(Transform origen)
    {
        Transform nearestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject player in playersInGame)
        {
            float distance = Vector2.Distance(origen.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestPlayer = player.transform;
            }
        }

        return nearestPlayer;
    }

}
