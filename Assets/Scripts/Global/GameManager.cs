using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public RoomGenerator roomGenerator;
    public PhotonView player;

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

    private void Update()
    {
        
    }
}
