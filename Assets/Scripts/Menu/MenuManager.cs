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
    public GameObject messageEmptyPlayerName;
    public GameObject messageEmptyRoomName;
    public Button crearSalaButton;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        PlayerPrefs.SetFloat("Volumen", 0.5f);
     
    }

    private void Update()
    {

        if (PhotonNetwork.InLobby)
            crearSalaButton.interactable = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor de Photon");
        PhotonNetwork.JoinLobby();
    }

    public void SetPlayerName()
    {
        PhotonNetwork.NickName = playerNameInput.text; 
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            if (!string.IsNullOrEmpty(playerNameInput.text))
            {
                RoomOptions roomOptions = new RoomOptions
                {
                    MaxPlayers = 2 
                };

                if (PhotonNetwork.InLobby)
                {
                    SetPlayerName(); 
                    PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
                }
                
            }
            else
            {
                messageEmptyPlayerName.SetActive(true);
            }
            
        }
        else
        {
            messageEmptyRoomName.SetActive(true);
        }
    }

    public void JoinRoom(string roomName)
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            SetPlayerName();
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            messageEmptyPlayerName.SetActive(true);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
   
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

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

        //// Asignar un valor inicial para CharacterIndex si no existe
        //if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("CharacterIndex"))
        //{
        //    ExitGames.Client.Photon.Hashtable initialProperties = new ExitGames.Client.Photon.Hashtable
        //    {
        //        { "CharacterIndex", 0 } // Valor inicial por defecto
        //    };
        //    PhotonNetwork.LocalPlayer.SetCustomProperties(initialProperties);
        //}

        //// Sincronizar propiedades del MasterClient con los nuevos jugadores
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        //    {
        //        if (player.CustomProperties.TryGetValue("CharacterIndex", out object characterIndex))
        //        {
        //            GetComponent<PhotonView>().RPC("SyncPlayerProperties", RpcTarget.AllBuffered, player.ActorNumber, (int)characterIndex);
        //        }
        //    }
        //}

        PhotonNetwork.LoadLevel("CharacterSelection"); // Cargar la escena de selección de personaje
    }

    //[PunRPC]
    //private void SyncPlayerProperties(int actorNumber, int characterIndex)
    //{
    //    Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
    //    if (player != null)
    //    {
    //        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
    //        {
    //            { "CharacterIndex", characterIndex }
    //        };
    //        player.SetCustomProperties(properties);
    //    }
    //}
}
