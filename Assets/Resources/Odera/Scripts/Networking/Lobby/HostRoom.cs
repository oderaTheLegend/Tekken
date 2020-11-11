using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class HostRoom : MonoBehaviourPunCallbacks
{
    //[SerializeField] private Text roomName;
    //public Button but;
    //private Text RoomName
    //{
    //    get { return roomName; }
    //}

    //private void Start()
    //{
    //    but.interactable = false;
    //}

    //public void ClickAndCreateRoom()
    //{
    //    if (!PhotonNetwork.IsConnected)
    //    {
    //        return;
    //    }

    //    RoomOptions o = new RoomOptions();
    //    o.MaxPlayers = 2;
    //    PhotonNetwork.JoinOrCreateRoom(RoomName.text, o, TypedLobby.Default);

    //    PhotonNetwork.NickName = PlayerNetworking.Instance.Name;

    //    Debug.Log("My Nickname is " + PhotonNetwork.LocalPlayer.NickName, this);
    //}

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("Created Room Successfully", this);
    //}

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log("Room Creation Failed " + message, this);
    //}

    //public override void OnJoinedLobby()
    //{
    //    but.interactable = true;
    //    Debug.Log("JoinedLobby");
    //}
}