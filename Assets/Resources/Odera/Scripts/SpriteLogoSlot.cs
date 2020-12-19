using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpriteLogoSlot : MonoBehaviourPunCallbacks
{
    public List<Image> logoSprite;

    private int minCharacterNo = 0;
    private int maxCharacterNo = 1;

    public GameObject selectManager;
    ScreenSelectManager s;

    public static bool selected;

    public static bool p1Chose;
    public static bool p2Chose;

    bool fadeOnceStart;

    bool startCharacterSelect;

    public Image slot;
    void Start()
    {
        StartCoroutine(StartSelect());

        s = selectManager.GetComponent<ScreenSelectManager>();

        logoSprite[0].sprite = s.characterLogo[0];
        logoSprite[1].sprite = s.characterLogo[1];
    }

    private void Update()
    {
        if (Mode.mode != Mode.Modes.Online)
        {
            if (!selected)
            {
                Directional();
                UpdatePosition();
            }
        }
        else
        {
            Directional();
            UpdatePosition();

            if (PlayerIndex.i.playerIndex >= 2)
            {
                if (CharacterSelectNetwork.instance.allCharactersSelected >= 2)
                {
                    photonView.RPC("StartGame", RpcTarget.All);
                }
            }
            else
            {
                if (CharacterSelectNetwork.instance.allCharactersSelected == 1)
                {
                    StartGame();
                }
            }
        }
    }

    #region Functions
    [PunRPC]
    public void RememberP2(int x)
    {
        s.p2CharacterCurrent += x;
    }
    [PunRPC]
    public void RememberP1(int x)
    {
        s.p1CharacterCurrent += x;
    }

    [PunRPC]
    public void StartGame()
    {
        if (!fadeOnceStart)
        {
            StartCoroutine(FadePartnerImage(false));
            StartCoroutine(EnterLevel());
            fadeOnceStart = true;
        }
    }

    IEnumerator EnterLevel()
    {
        yield return new WaitForSeconds(4f);
        UIJuice.instance.GroupAlphaLerp(ScreenSelectManager.i.fadeCanvas.GetComponent<CanvasGroup>(), 0, 0.75f);
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(2);
    }

    IEnumerator FadePartnerImage(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1f; i >= 0; i -= 2f * Time.deltaTime)
            {
                ScreenSelectManager.i.partner.color = new Color(ScreenSelectManager.i.partner.color.r, ScreenSelectManager.i.partner.color.g, ScreenSelectManager.i.partner.color.b, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += 1.5f * Time.deltaTime)
            {
                ScreenSelectManager.i.partner.color = new Color(ScreenSelectManager.i.partner.color.r, ScreenSelectManager.i.partner.color.g, ScreenSelectManager.i.partner.color.b, i);
                yield return null;
            }
        }
    }
    public void Directional()
    {
        //Keyboard Controls
        if (Mode.mode != Mode.Modes.Online)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (CharacterSelectNetwork.instance.analogKeyPad.left)
                {
                    s.p1CharacterCurrent--;
                    CharacterSelectNetwork.instance.analogKeyPad.left = false;
                }

                if (CharacterSelectNetwork.instance.analogKeyPad.right)
                {
                    s.p1CharacterCurrent++;
                    CharacterSelectNetwork.instance.analogKeyPad.right = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    s.p1CharacterCurrent--;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    s.p1CharacterCurrent++;
                }
            }
        }
        else
        {
            if (startCharacterSelect)
            {
                if (photonView.IsMine)
                {
                    if (!p1Chose)
                    {
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            if (CharacterSelectNetwork.instance.analogKeyPad.right)
                            {
                                photonView.RPC("RememberP1", RpcTarget.All, 1);
                            }

                            if (CharacterSelectNetwork.instance.analogKeyPad.left)
                            {
                                photonView.RPC("RememberP1", RpcTarget.All, -1);
                            }
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                photonView.RPC("RememberP1", RpcTarget.All, -1);
                            }

                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                photonView.RPC("RememberP1", RpcTarget.All, 1);
                            }
                        }
                    }
                }
                else
                {
                    if (!p2Chose)
                    {
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            if (CharacterSelectNetwork.instance.joyStick.right)
                            {
                                photonView.RPC("RememberP2", RpcTarget.All, 1);
                            }

                            if (CharacterSelectNetwork.instance.joyStick.left)
                            {
                                photonView.RPC("RememberP2", RpcTarget.All, -1);
                            }
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                photonView.RPC("RememberP2", RpcTarget.All, -1);
                            }

                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                photonView.RPC("RememberP2", RpcTarget.All, 1);
                            }
                        }
                    }
                }
            }
        }
    }
    IEnumerator StartSelect()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            slot.color = Color.red;
            yield return new WaitForSeconds(3);
            startCharacterSelect = true;
            slot.color = Color.blue;
        }
    }

    void UpdatePosition()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            if (startCharacterSelect)
            {
                if (photonView.IsMine)
                {
                    if (s.p1CharacterCurrent < minCharacterNo)
                    {
                        s.p1CharacterCurrent = minCharacterNo;
                    }
                    else if (s.p1CharacterCurrent > maxCharacterNo)
                    {
                        s.p1CharacterCurrent = maxCharacterNo;
                    }
                    else
                    {
                        s.slotPos.transform.position = logoSprite[s.p1CharacterCurrent].transform.position;
                    }
                }
                else
                {
                    if (s.p2CharacterCurrent < minCharacterNo)
                    {
                        s.p2CharacterCurrent = minCharacterNo;
                    }
                    else if (s.p2CharacterCurrent > maxCharacterNo)
                    {
                        s.p2CharacterCurrent = maxCharacterNo;
                    }
                    else
                    {
                        s.slotPos.transform.position = logoSprite[s.p2CharacterCurrent].transform.position;
                    }
                }
            }
        }
        else
        {
            if (s.p1CharacterCurrent < minCharacterNo)
            {
                s.p1CharacterCurrent = minCharacterNo;
            }
            else if (s.p1CharacterCurrent > maxCharacterNo)
            {
                s.p1CharacterCurrent = maxCharacterNo;
            }
            else
            {
                s.slotPos.transform.position = logoSprite[s.p1CharacterCurrent].transform.position;
            }
        }
    }
    #endregion
}