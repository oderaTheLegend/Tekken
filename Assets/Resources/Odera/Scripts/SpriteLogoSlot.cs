using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLogoSlot : MonoBehaviour
{
    public List<Image> logoSprite;

    private int minCharacterNo = 0;
    private int maxCharacterNo = 3;

    public GameObject selectManager;
    ScreenSelectManager s;

    public static bool selected;

    void Start()
    {
        s = selectManager.GetComponent<ScreenSelectManager>();

        logoSprite[0].sprite = s.characterLogo[0];
        logoSprite[1].sprite = s.characterLogo[1];
        logoSprite[2].sprite = s.characterLogo[2];
        logoSprite[3].sprite = s.characterLogo[3];
    }

    private void Update()
    {
        if (!selected) { 
        Directional();
        UpdatePosition();
        }
    }

    /*Functions*/
    #region
    public void CharacterNum(int x)
    {
        if (!selected) { 
        s.currentCharacter = x;
        }
    }

    public void Directional()
    {
        //Keyboard Controls
        if (Input.GetKeyDown(KeyCode.A))
            s.currentCharacter--;

        if (Input.GetKeyDown(KeyCode.D))
            s.currentCharacter++;     
    }

    void UpdatePosition()
    {
        //Updating Position
        if (s.currentCharacter < minCharacterNo)
            s.currentCharacter = minCharacterNo;
        else if (s.currentCharacter > maxCharacterNo)
            s.currentCharacter = maxCharacterNo;
        else
            s.slotPos.transform.position = logoSprite[s.currentCharacter].transform.position;
    }
    #endregion
}