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
    public List<GameObject> floorPrefabs; // Prefab del suelo
    public GameObject floorParent; // Prefab del suelo
    public GameObject wallPrefab;  // Prefab del muro
    public GameObject wallParent;  // Prefab del muro

    [Header("Spawner")]
    public GameObject spawnerPrefab;

    private GameObject spawner;


    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        //spawner.SpawnEnemigos();
        GenerateRoom();

        if (PhotonNetwork.IsMasterClient)
        {

              StartCoroutine(SpawnCount());

        }

        //tenemos que cvonvertir el spawner en un photonview para que se sincronice entre jugadores, y así que spawneen los mismos enemigos para todos
        //luego tenemos que instanciar con photon ese objeto,y luego le pasaremos los dos players in game a los que pueda seguir continuamente segun la cercanía

    }
   
    IEnumerator SpawnCount()
    {
        spawner = PhotonNetwork.Instantiate(spawnerPrefab.name, Vector3.zero, Quaternion.identity);
        SpawnEnemies spawnEnemies = spawner.GetComponent<SpawnEnemies>();
        spawnEnemies.SetRoomGenerator(this);

        yield return new WaitForSeconds(2);
        spawner.GetComponent<PhotonView>().RPC("SpawnEnemigos", RpcTarget.All);

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
                    int rnd = Random.Range(0, floorPrefabs.Count);
                    Instantiate(floorPrefabs[rnd], position, Quaternion.identity, floorParent.transform);

                }

            }
        }
    }

}
