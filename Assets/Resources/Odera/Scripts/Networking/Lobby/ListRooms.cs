using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ListRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform content;
    [SerializeField] private RoomListPrefab roomListing;

    private List<RoomListPrefab> listing = new List<RoomListPrefab>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = listing.FindIndex(x => x.RoomI.Name == info.Name); //What listing has the same room name
                if(index != -1)
                {
                    Destroy(listing[index].gameObject);
                    listing.RemoveAt(index);
                }
            }
            else
            {
                RoomListPrefab list = (RoomListPrefab)Instantiate(roomListing, content);
                if (list != null)
                {
                    list.SetRoomInfo(info);
                    listing.Add(list);
                }
            }
            
        }
    }
}