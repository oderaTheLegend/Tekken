using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public int lifeHealth;

    public bool GameOver;
    public GameObject commonItems;
    public GameObject fadeCanvas;
    public GameObject mainCanvas;

    public Transform p1DetailsPos;
    public Transform p2DetailsPos;

    public Text p1Name;

    public GameObject playerChain;
    public GameObject playerSaw;

    [HideInInspector] public Joystick joystick;
    public GameObject stick;

    public GameObject mobileInventory;

    public static GameManager instance;

    int howMany;
    void Start()
    {
        instance = this;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (stick != null)
            {
                joystick = stick.GetComponent<Joystick>();
            }

            mobileInventory.SetActive(true);
        }
        else
        {
            mobileInventory.SetActive(false);
        }

        UIJuice.instance.GroupAlphaLerp(fadeCanvas.GetComponent<CanvasGroup>(), 0, 1);

        if (Mode.mode == Mode.Modes.Training)
        {
            commonItems.SetActive(true);

            if (ScreenSelectManager.i.p1CharacterCurrent == 0)
            {
                Instantiate(playerChain, new Vector3(-2.39899993f, -1.25899994f, -0.600000024f), Quaternion.identity);
            //    p1Name.text = "Chain";
            }
            else
            {
                Instantiate(playerSaw, new Vector3(-2.39899993f, -1.25899994f, -0.600000024f), Quaternion.identity);
              //  p1Name.text = "Saw";
            }
        }
        else if (Mode.mode == Mode.Modes.Online)
        {
            if (ScreenSelectManager.i.p1CharacterCurrent == 0)
            {
                PhotonNetwork.Instantiate("Player_01", new Vector3(-2.39899993f, -1.25899994f, -0.600000024f), Quaternion.identity);
            }
            else
            {
                Debug.Log("ok");
                PhotonNetwork.Instantiate("Player_02", new Vector3(-2.39899993f, -1.25899994f, -0.600000024f), Quaternion.identity);
            }
          

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
        else
        {
            if (ScreenSelectManager.i.p1CharacterCurrent == 0)
            {
                Instantiate(playerChain, new Vector3(-2.39899993f, -1.25899994f, -0.600000024f), Quaternion.identity);
             //   p1Name.text = "Chain";
            }
            else
            {
                Instantiate(playerSaw, new Vector3(-2.39899993f, -1.25899994f, -0.600000024f), Quaternion.identity);
             //   p1Name.text = "Saw";
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