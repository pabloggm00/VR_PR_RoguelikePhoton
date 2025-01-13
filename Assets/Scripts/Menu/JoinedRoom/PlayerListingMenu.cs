using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private GameObject _playerListing;

    public GameObject playButton;

    private List<PlayerListing> _listings = new List<PlayerListing>();


    private void Awake()
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {

        foreach (PlayerListing listing in _listings)
        {
            Destroy(listing.gameObject);
        }

        _listings.Clear();

        if (PhotonNetwork.CurrentRoom == null)
            return;

        foreach (KeyValuePair<int, Player> infoPlayer in PhotonNetwork.CurrentRoom.Players)
        {
            GameObject newPlayerObject = Instantiate(_playerListing, _content);
            PlayerListing newPlayer = newPlayerObject.GetComponentInChildren<PlayerListing>();

            newPlayer.SetPlayerInfo(infoPlayer.Value);

            if (infoPlayer.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayer.ApplyLocalChanges();
            }

            _listings.Add(newPlayer);

        }
    }

   

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //AddPlayerListing(newPlayer);
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //DestroyPlayer(otherPlayer);
        UpdatePlayerList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            playButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(false);
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("Gameplay");
    }

}
