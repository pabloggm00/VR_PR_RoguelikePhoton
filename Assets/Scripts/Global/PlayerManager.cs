using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager instance;

    public List<GameObject> playersInGame = new List<GameObject>(); // Lista sincronizada de jugadores

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

   

    [PunRPC]
    public void AddPlayerToNetwork(int viewID)
    {
        GameObject player = PhotonView.Find(viewID)?.gameObject;
        if (player != null && !playersInGame.Contains(player))
        {
            playersInGame.Add(player);
        }
    }

    public void RegisterPlayer(GameObject player)
    {
        if (!playersInGame.Contains(player))
        {
            playersInGame.Add(player);

            // Sincronizar con todos los clientes usando el ViewID
            int viewID = player.GetComponent<PhotonView>().ViewID;
            photonView.RPC("AddPlayerToNetwork", RpcTarget.OthersBuffered, viewID);
        }
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
