using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public GameObject playerManagerPrefab;
    public GameObject playerPrefab;
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;
    //public GameObject poolParent;
    public int soulsNeeded = 10;

  

    public List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        instance = this;

        // Instanciar el PlayerManager si no existe
        if (PlayerManager.instance == null)
        {
            PhotonNetwork.Instantiate(playerManagerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            
            SpawnPlayer();
        }
      
    }

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Muerto").Length == 2)
        {
            PhotonNetwork.LoadLevel("Menu");
        }
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

        // Registrar el jugador en el PlayerManager
        PlayerManager.instance.RegisterPlayer(playerObject);
        players.Add(playerObject);
        
    }


    private Transform GetSpawnPoint(Photon.Realtime.Player player )
    {
        if (player.IsMasterClient)
        {
            return spawnPointPlayer1;
        }

        return spawnPointPlayer2;
    }


    

}
