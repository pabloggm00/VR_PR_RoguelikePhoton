using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();


    private void Awake()
    {
        GetCurrentPlayers();
    }

    private void GetCurrentPlayers()
    {
        Debug.Log(PhotonNetwork.CurrentRoom);
        foreach (KeyValuePair<int, Player> infoPlayer in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(infoPlayer.Value);
        }

    }

    private void AddPlayerListing(Player player)
    {
        GameObject listingObjct = PhotonNetwork.Instantiate(_playerListing.name, _content.position, _content.rotation) ;

        PlayerListing listing = listingObjct.GetComponent<PlayerListing>();

        listingObjct.transform.SetParent(_content, false);

        if (listing != null)
        {
            //listing.SetPlayerInfo(player);
            listing.GetComponent<PhotonView>().RPC("SetPlayerInfo", RpcTarget.All, player);
            _listings.Add(listing);
        }
    }

    private void DestroyPlayer(Player player)
    {
        int index = _listings.FindIndex(x => x.player == player);

        if (index != 1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DestroyPlayer(otherPlayer);
    }
}
