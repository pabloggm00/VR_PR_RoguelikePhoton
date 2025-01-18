using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public RoomGenerator roomGenerator;
    public PhotonView player;

    public GameObject poolParent;
    public int soulsNeeded = 5;

    public List<GameObject> playersInGame = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        InitWorld();
    }

    void InitWorld()
    {
        // roomGenerator.GenerateRoom();
        roomGenerator.InitGame(player);
    }

    public void AgregarJugador(GameObject player)
    {
        playersInGame.Add(player);
    }

}
