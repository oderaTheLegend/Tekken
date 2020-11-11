using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPrefab : MonoBehaviour
{
    [SerializeField] private Text text;

    public RoomInfo info;

    public RoomInfo RoomI { get; set; }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomI = roomInfo;
        //Layout: Max Players: 2 | Name: Odera
        text.text = "Max Players: " + roomInfo.MaxPlayers + " | " + roomInfo.Name;
    }
}
