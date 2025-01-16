using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("Medidas")]
    public int width;
    public int height;

    [Header("Prefabs")]
    public GameObject floorPrefab; // Prefab del suelo
    public GameObject floorParent; // Prefab del suelo
    public GameObject wallPrefab;  // Prefab del muro
    public GameObject wallParent;  // Prefab del muro

    [Header("Spawner")]
    public SpawnEnemies spawner;
    public Transform spawnPlayer;

    GameObject SpawnPlayer(PhotonView playerPrefab)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        return player;
    }

    public void InitGame(PhotonView player)
    {
        spawner.player = SpawnPlayer(player);
        spawner.SpawnEnemigos();

        GenerateRoom();
    }

    public void GenerateRoom()
    {

        // Centro de la sala
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        for (int x = -halfWidth; x <= halfWidth; x++) 
        {
            for (int y = -halfHeight; y <= halfHeight; y++)
            {
                Vector3 position = new Vector3(x, y, x);

                if (x == -halfWidth || x == halfWidth || y == -halfHeight || y == halfHeight)
                {

                    Instantiate(wallPrefab, position, Quaternion.identity, wallParent.transform);
                }
                else
                {

                    Instantiate(floorPrefab, position, Quaternion.identity, floorParent.transform);

                }

            }
        }
    }

}
