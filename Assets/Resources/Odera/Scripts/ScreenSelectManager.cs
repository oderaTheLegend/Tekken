using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class Mode
{
    public enum Modes { Training, Online, Story }
    public static Modes mode;

    public static int currentP1Name;
    public static int currentP2Name;
}

public class ScreenSelectManager : MonoBehaviourPunCallbacks
{
    public Sprite dummy;
    public Image dummyImage;

    public List<Sprite> characterSprite;
    public List<Sprite> characterLogo;
    public List<string> characterName;

    [Tooltip("Keep track of current character, so you are able to re-choose characters too")]
    public List<string> playableCharacter;

    public Image[] playerImages;
    public Text[] playerNames;

    public Image versus;
    public Image partner;

    [Tooltip("This is the square select thing. (Play the game, you'll see)")]
    public GameObject slotPos;
    public GameObject fadeCanvas;

    [HideInInspector]
    public int p1CharacterCurrent;
    [HideInInspector]
    public int p2CharacterCurrent;

    public Text disconnectedText;

    public bool canSelectP1;
    public bool canSelectP2;

    public static ScreenSelectManager i;

    public GameObject mobileInventory;

    private void Start()
    {
        i = this;

        if (fadeCanvas != null)
        {
            UIJuice.instance.GroupAlphaLerp(fadeCanvas.GetComponent<CanvasGroup>(), 1, 2);
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            mobileInventory.SetActive(true);
        }
        else
        {
            mobileInventory.SetActive(false);
        }

        disconnectedText.text = "";
        p1CharacterCurrent = 0;
        p2CharacterCurrent = 0;

        Hashtable props = new Hashtable { { InGame.playerLoaded, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (Mode.mode == Mode.Modes.Online)
        {
            if (PlayerIndex.i.playerIndex >= 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    playerNames[0].text = "P2 Deciding";
                }
                else
                {
                    playerNames[1].text = "P1 Deciding";
                }
            }
            else
            {
                playerNames[0].text = "Pick Your Character";
            }
        }

        if (Mode.mode == Mode.Modes.Story)
        {
            playerNames[0].text = "Pick Your Character";
            playerImages[0].enabled = false;
        }
    }

    private void Update()
    {
        PlayerDetails();
        ChooseCharacter();
        GameMode();
    }

    #region Photon

    public override void OnLeftRoom()
    {
        Hashtable props = new Hashtable
            {
                {InGame.playerDisconnected, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        DisconnectPlayers();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.LeaveRoom();
    }

    private bool DisconnectPlayers()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerDisconnected;

            if (p.CustomProperties.TryGetValue(InGame.playerDisconnected, out playerDisconnected))
            {
                if (!(bool)playerDisconnected)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        StartCoroutine(Disconnected());
        return true;
    }
    #endregion

    #region Functions
    void PlayerDetails()
    {
        if (Mode.mode != Mode.Modes.Online)
        {
            playerImages[1].sprite = characterSprite[p1CharacterCurrent];
            playerNames[1].text = characterName[p1CharacterCurrent];
        }
    }  

    public void ChooseCharacter()
    {
        //Keyboard Input
        if (Mode.mode != Mode.Modes.Online)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (CharacterSelectNetwork.instance.joyStick.selected)
                {
                    SpriteLogoSlot.selected = true;

                    StartCoroutine(FadeVersus(false));

                    playableCharacter.Add(characterName[p1CharacterCurrent]);

                    if (playableCharacter.Count >= 2)
                    {
                        playableCharacter.RemoveAt(0);
                    }
                }

                if (CharacterSelectNetwork.instance.joyStick.unselected)
                {
                    if (playableCharacter.Count == 1)
                    {
                        StopAllCoroutines();
                        SpriteLogoSlot.selected = false;
                        StartCoroutine(FadeVersus(true));
                        playableCharacter.RemoveAt(0);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SpriteLogoSlot.selected = true;

                    StartCoroutine(FadeVersus(false));

                    playableCharacter.Add(characterName[p1CharacterCurrent]);

                    if (playableCharacter.Count >= 2)
                    {
                        playableCharacter.RemoveAt(0);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (playableCharacter.Count == 1)
                    {
                        StopAllCoroutines();
                        SpriteLogoSlot.selected = false;
                        StartCoroutine(FadeVersus(true));
                        playableCharacter.RemoveAt(0);
                    }
                }
            }

            StartCoroutine(EnterLevel(2));
        }
    }

    void GameMode()
    {
        if (Mode.mode == Mode.Modes.Training)
        {
            playerNames[0].text = "Training Dummy";
            dummyImage.sprite = dummy;
        }
    }

    IEnumerator Disconnected()
    {
        disconnectedText.text = "Sorry, the other player disconnected. Returning to main menu...";
        yield return new WaitForSeconds(3);
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.Disconnect();
    }

    IEnumerator EnterLevel(int level)
    {
        yield return new WaitForSeconds(4);

        if (SpriteLogoSlot.selected)
        {
            UIJuice.instance.GroupAlphaLerp(fadeCanvas.GetComponent<CanvasGroup>(), 0, 0.75f);
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene(level);
        }
    }

    IEnumerator FadeVersus(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1f; i >= 0; i -= 2f * Time.deltaTime)
            {
                versus.color = new Color(versus.color.r, versus.color.g, versus.color.b, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += 1.5f * Time.deltaTime)
            {
                versus.color = new Color(versus.color.r, versus.color.g, versus.color.b, i);
                yield return null;
            }
        }
    }

    #endregion Functions
}