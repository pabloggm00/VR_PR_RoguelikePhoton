using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RoomGenerator roomGenerator;
    public PhotonView player;

    private void Start()
    {

        InitWorld();
    }

    void InitWorld()
    {
        roomGenerator.GenerateRoom();
        //roomGenerator.InitGame(player);
    }

    private void Update()
    {
        
    }
}
