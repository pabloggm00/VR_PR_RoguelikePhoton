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
    public GameObject spawnerPrefab;
    public Transform spawnPlayer;

    private GameObject spawner;

    GameObject SpawnPlayer(PhotonView playerPrefab)
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerSetup>().IsLocalPlayer();
        player.GetComponent<PhotonView>().RPC("Init", RpcTarget.All, (int)PhotonNetwork.LocalPlayer.CustomProperties["playerSprite"]);
        player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.All, (string)PhotonNetwork.LocalPlayer.CustomProperties["Nickname"]);
        player.GetComponent<PhotonView>().RPC("SetPoolParent", RpcTarget.All, (string)PhotonNetwork.LocalPlayer.CustomProperties["Nickname"]);

        return player;
    }

    public void InitGame(PhotonView player)
    {
        GameplayManager.instance.AgregarJugador(SpawnPlayer(player));

        //spawner.SpawnEnemigos();
        GenerateRoom();

        StartCoroutine(SpawnCount());
        //tenemos que cvonvertir el spawner en un photonview para que se sincronice entre jugadores, y así que spawneen los mismos enemigos para todos
        //luego tenemos que instanciar con photon ese objeto,y luego le pasaremos los dos players in game a los que pueda seguir continuamente segun la cercanía

    }
   
    IEnumerator SpawnCount()
    {
        spawner = PhotonNetwork.Instantiate(spawnerPrefab.name, Vector3.zero, Quaternion.identity);
        var spawnEnemies = spawner.GetComponent<SpawnEnemies>();
        //spawnEnemies.SetRoomGenerator(GameplayManager.instance.roomGenerator);

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

                    Instantiate(floorPrefab, position, Quaternion.identity, floorParent.transform);

                }

            }
        }
    }

}
