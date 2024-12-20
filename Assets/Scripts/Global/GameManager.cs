using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RoomGenerator roomGenerator;
    public GameObject player;

    private void Start()
    {
       InitWorld();
    }

    void InitWorld()
    {
        roomGenerator.GenerateRoom();
        roomGenerator.SpawnPlayer(player);

    }
}
