using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public TMP_InputField playerNameInput;
    public TMP_InputField roomNameInput;
    public Transform roomListContent;
    public GameObject roomListItemPrefab;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings(); // Conectar al servidor Photon
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor de Photon");
        PhotonNetwork.JoinLobby(); // Unirse al lobby principal
    }

    public void SetPlayerName()
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            PhotonNetwork.NickName = playerNameInput.text; // Configurar nombre del jugador
        }
        else
        {
            Debug.LogWarning("El nombre del jugador no puede estar vacío.");
        }
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 2 // Máximo de jugadores en la sala
            };

            SetPlayerName(); // Asegurarse de que el nombre esté configurado antes de crear la sala
            PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
        }
        else
        {
            Debug.LogWarning("El nombre de la sala no puede estar vacío.");
        }
    }

    public void JoinRoom(string roomName)
    {
        SetPlayerName();
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Limpiar la lista de salas anteriores
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        // Mostrar las salas disponibles
        foreach (RoomInfo room in roomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListContent);
            roomItem.GetComponentInChildren<TMP_Text>().text = room.Name;

            roomItem.GetComponent<Button>().onClick.AddListener(() => JoinRoom(room.Name));
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Unido a la sala: " + PhotonNetwork.CurrentRoom.Name);

        // Asignar un valor inicial para CharacterIndex si no existe
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("CharacterIndex"))
        {
            ExitGames.Client.Photon.Hashtable initialProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "CharacterIndex", 0 } // Valor inicial por defecto
        };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProperties);
        }

        PhotonNetwork.LoadLevel("CharacterSelection"); // Cargar la escena de selección de personaje
    }
}
