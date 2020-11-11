using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject commonItems;
    public GameObject networkingItems;
    public GameObject mainCanvas;
    public GameObject fadeCanvas;

    public Text player1Nickname, player2Nickname;
    public Text player1, player2;

    Player p1;
    Player p2;

    void Start()
    {
       PhotonNetwork.Instantiate("Player", new Vector3(0, 0.0147f, 0), Quaternion.identity, 0);

        p1 = PhotonNetwork.LocalPlayer;
        p2 = PhotonNetwork.LocalPlayer.GetNext();

        UIJuice.instance.GroupAlphaLerp(mainCanvas.GetComponent<CanvasGroup>(), 1, 1);
        UIJuice.instance.GroupAlphaLerp(fadeCanvas.GetComponent<CanvasGroup>(), 0, 1);

        commonItems.SetActive(true);

        if (Mode.mode == Mode.Modes.Training)
        {
            networkingItems.SetActive(false);
        }
        else
        {
            player1.text = ScreenSelectManager.i.characterName[Mode.currentP1Name];
            player2.text = ScreenSelectManager.i.characterName[Mode.currentP2Name];

            //  p.GetComponent<PhotonView>().RPC("SetNames", RpcTarget.All);
            networkingItems.SetActive(true);
        }
    }


    [PunRPC]
    public void SetNames()
    {
        player1Nickname.text = p1.NickName;
        player2Nickname.text = p2.NickName;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
}
