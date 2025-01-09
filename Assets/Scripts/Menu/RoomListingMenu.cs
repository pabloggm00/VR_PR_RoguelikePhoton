using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    public TMP_Text _playerName;

    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Lista actualizada");
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.roomInfo.Name == info.Name);

                if (index != 1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                RoomListing listing = Instantiate(_roomListing, _content);
            
                if (listing != null)
                {
                    listing.SetRoomInfo(info, _playerName);
                    _listings.Add(listing);
                }
            }


            
        }
    }
}
