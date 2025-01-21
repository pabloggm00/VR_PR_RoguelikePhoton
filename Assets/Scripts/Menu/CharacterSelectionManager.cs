using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public Transform playerListContent; 
    public GameObject playerListItemPrefab; 
    public Button buttonJugar;

    [Header("Sprites de Personaje")]
    public PlayerSprites playerSprites; 

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonJugar.interactable = true;
        }

        UpdatePlayerList();
    }


    private void UpdatePlayerList()
    {
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerItem = Instantiate(playerListItemPrefab, playerListContent);
            PlayerListItem itemScript = playerItem.GetComponent<PlayerListItem>();

            
            itemScript.SetPlayer(player);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
      
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu"); 
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.NickName} se unió a la sala.");
        UpdatePlayerList(); 
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.NickName} salió de la sala.");
        UpdatePlayerList(); 
    }

    /*public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log($"Propiedades de {targetPlayer.NickName} actualizadas.");
        UpdatePlayerList(); // Asegúrate de que la lista de jugadores refleje los cambios
    }*/

}
