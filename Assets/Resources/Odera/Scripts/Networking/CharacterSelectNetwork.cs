using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectNetwork : MonoBehaviourPun
{
    public int rememberCharacter;
    public static CharacterSelectNetwork instance;

    public int allCharactersSelected;

    int currentCharacterView;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (PlayerIndex.i.playerIndex >= 2)
                {
                    Mode.currentP1Name = ScreenSelectManager.i.p1CharacterCurrent;
                    Mode.currentP2Name = ScreenSelectManager.i.p2CharacterCurrent;

                    photonView.RPC("Select", RpcTarget.Others);

                    if (photonView.IsMine)
                    {
                        if (currentCharacterView >= 2)
                        {
                            FinishedSelecting();
                            SpriteLogoSlot.p1Chose = true;
                        }
                        else
                        {
                            if (!SpriteLogoSlot.p1Chose)
                            {
                                currentCharacterView++;
                                rememberCharacter++;
                                allCharactersSelected++;
                                SpriteLogoSlot.p1Chose = true;
                            }
                        }
                    }
                    else
                    {
                        if (rememberCharacter < 1)
                        {
                            if (!SpriteLogoSlot.p2Chose)
                            {
                                photonView.RPC("FinishedSelecting", RpcTarget.MasterClient);
                                SpriteLogoSlot.p2Chose = true;
                            }
                        }
                    }

                    if (PhotonNetwork.IsMasterClient)
                    {
                        ScreenSelectManager.i.playerImages[1].sprite = ScreenSelectManager.i.characterSprite[ScreenSelectManager.i.p1CharacterCurrent];
                        ScreenSelectManager.i.playerNames[1].text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p1CharacterCurrent];
                    }
                    else
                    {
                        ScreenSelectManager.i.playerImages[0].sprite = ScreenSelectManager.i.characterSprite[ScreenSelectManager.i.p2CharacterCurrent];
                        ScreenSelectManager.i.playerNames[0].text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p2CharacterCurrent];
                    }
                }
                else
                {
                    Mode.currentP1Name = ScreenSelectManager.i.p1CharacterCurrent;

                    photonView.RPC("Select", RpcTarget.Others);

                    if (photonView.IsMine)
                    {
                        if (!SpriteLogoSlot.p1Chose)
                        {
                            currentCharacterView++;
                            FinishedSelecting();
                            SpriteLogoSlot.p1Chose = true;
                        }
                    }

                    ScreenSelectManager.i.playerImages[1].sprite = ScreenSelectManager.i.characterSprite[ScreenSelectManager.i.p1CharacterCurrent];
                    ScreenSelectManager.i.playerNames[1].text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p1CharacterCurrent];
                }
            }          
        }
    }

    [PunRPC]
    public void Select()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ScreenSelectManager.i.playerImages[0].sprite = ScreenSelectManager.i.characterSprite[ScreenSelectManager.i.p2CharacterCurrent];
            ScreenSelectManager.i.playerNames[0].text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p2CharacterCurrent];
        }
        else
        {
            ScreenSelectManager.i.playerImages[1].sprite = ScreenSelectManager.i.characterSprite[ScreenSelectManager.i.p1CharacterCurrent];
            ScreenSelectManager.i.playerNames[1].text = ScreenSelectManager.i.characterName[ScreenSelectManager.i.p1CharacterCurrent];
        }
    }

    [PunRPC]
    public void FinishedSelecting()
    {
        rememberCharacter++;
        allCharactersSelected++;
    }
}