using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public Transform playerListContent; // Contenedor de la lista de jugadores
    public GameObject playerListItemPrefab; // Prefab de cada entrada de jugador en la lista
    public Button buttonJugar;

    [Header("Sprites de Personaje")]
    public PlayerSprites playerSprites; // ScriptableObject con los sprites disponibles

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonJugar.interactable = true;
        }

        UpdatePlayerList();
    }

    /// <summary>
    /// Actualiza la lista de jugadores en la sala.
    /// </summary>
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

            // Configurar la entrada del jugador
            itemScript.SetPlayer(player, playerSprites);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu"); // Volver al menú de lista de salas
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Actualizar la lista si cambian las propiedades de algún jugador
        UpdatePlayerList();
    }
}
