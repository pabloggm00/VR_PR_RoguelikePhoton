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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerListing listing = Instantiate(_playerListing, _content);

        if (listing != null)
        {
            listing.SetPlayerInfo(newPlayer);
            _listings.Add(listing);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.player == otherPlayer);

        if (index != 1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }
}
