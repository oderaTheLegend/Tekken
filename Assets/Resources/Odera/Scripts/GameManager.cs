using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject commonItems;
    public GameObject fadeCanvas;
    public GameObject mainCanvas;

    public Transform p1DetailsPos;
    public Transform p2DetailsPos;

    void Start()
    {
        PhotonNetwork.Instantiate("Cammy!", new Vector3(Random.Range(0, 5), 0.0147f, Random.Range(0, 5)), Quaternion.identity);

        UIJuice.instance.GroupAlphaLerp(fadeCanvas.GetComponent<CanvasGroup>(), 0, 1);

        if (Mode.mode == Mode.Modes.Training)
        {
            commonItems.SetActive(true);
        }
        else
        {
            commonItems.SetActive(false);

            if (PlayerIndex.i.playerIndex >= 2)
            {
                GameObject P1 = PhotonNetwork.Instantiate("Player1Details", p1DetailsPos.position, Quaternion.identity, 0);
                GameObject P2 = PhotonNetwork.Instantiate("Player2Details", p2DetailsPos.position, Quaternion.identity, 0);

                P1.GetComponentInChildren<Text>().text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p1CharacterCurrent];
                P2.GetComponentInChildren<Text>().text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p2CharacterCurrent];

                P1.transform.parent = mainCanvas.transform;
                P2.transform.parent = mainCanvas.transform;

                P1.transform.localScale = new Vector3(1, 1, 1);
                P2.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                GameObject P1 = PhotonNetwork.Instantiate("Player1Details", p1DetailsPos.position, Quaternion.identity);

                P1.GetComponentInChildren<Text>().text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p1CharacterCurrent];

                P1.transform.parent = mainCanvas.transform;
                P1.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            PhotonNetwork.RemoveRPCs(p);
            PhotonNetwork.DestroyPlayerObjects(p);
        }
    }
}