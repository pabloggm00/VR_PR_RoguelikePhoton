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
    public GameObject doorPrefab;  // Prefab de la puerta
    public GameObject doorParent;  // Prefab de la puerta

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

                    // No colocar muros donde van las puertas
                    if ((y == 0 && (x == -halfWidth || x == halfWidth)) ||
                        (x == 0 && (y == -halfHeight || y == halfHeight)))
                    {
                        continue;
                    }

                    Instantiate(wallPrefab, position, Quaternion.identity, wallParent.transform);
                }
                else
                {

                    Instantiate(floorPrefab, position, Quaternion.identity, floorParent.transform);

                }

            }
        }

        // Colocar puertas (centradas en cada pared)
        CreateDoor(new Vector3(0, halfHeight, 0));   // Puerta superior
        CreateDoor(new Vector3(0, -halfHeight, 0));  // Puerta inferior
        CreateDoor(new Vector3(halfWidth, 0, 0));    // Puerta derecha
        CreateDoor(new Vector3(-halfWidth, 0, 0));   // Puerta izquierda
    }

    void CreateDoor(Vector3 position)
    {
        Instantiate(doorPrefab, position, Quaternion.identity, doorParent.transform);
    }
}
