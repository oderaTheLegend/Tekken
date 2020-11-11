using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetworkSettings : MonoBehaviourPunCallbacks
{
    public GameObject menu;
    public GameObject networkLoadingGO;

    public Text infoText;
    public GameObject loading;

    public static NetworkSettings i;

    private void Start()
    {
        i = this;
        networkLoadingGO.SetActive(false);
    }

    public void JoinGame()
    {
        
        networkLoadingGO.SetActive(true);
        UIJuice.instance.GroupAlphaLerp(menu.GetComponent<CanvasGroup>(), 0, 2);
        UIJuice.instance.GroupAlphaLerp(networkLoadingGO.GetComponent<CanvasGroup>(), 1, 1);

        PhotonNetwork.JoinLobby();

      
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRoom("MyRoom");
    }

    public void Back()
    {
        StartCoroutine(SetLoadingGO());
    }

    void UnableToFindRoom()
    {
        GetComponent<PhotonView>().RPC("UpdateOtherText", RpcTarget.All, "Searching for a match");
        loading.SetActive(true);
    }

    IEnumerator SetLoadingGO()
    {
        UIJuice.instance.GroupAlphaLerp(networkLoadingGO.GetComponent<CanvasGroup>(), 0, 1);
        yield return new WaitForSeconds(1);
        infoText.text = "Searching for a match";
        networkLoadingGO.SetActive(false);
        UIJuice.instance.GroupAlphaLerp(menu.GetComponent<CanvasGroup>(), 1, 2);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        UnableToFindRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        GetComponent<PhotonView>().RPC("UpdateOtherText", RpcTarget.All, "Unable to create a room");
        StartCoroutine(SetLoadingGO());
    }

    public override void OnJoinedRoom()
    {
        GetComponent<PhotonView>().RPC("UpdateOtherText", RpcTarget.All, "Waiting for incoming connections");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            GetComponent<PhotonView>().RPC("UpdateOtherText", RpcTarget.All, "Found Player");
            GetComponent<PhotonView>().RPC("Load", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UpdateOtherText(string sentence)
    {
        infoText.text = sentence.ToString();
    }
    [PunRPC]
    public void Load()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (returnCode == 32758)
        {
            RoomOptions o = new RoomOptions
            {
                MaxPlayers = 2
            };
            PhotonNetwork.CreateRoom("MyRoom", o);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UnableToFindRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnableToFindRoom();
    }
}