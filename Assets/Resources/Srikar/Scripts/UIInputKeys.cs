using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputKeys : MonoBehaviour
{
    [Header("Serialized Variables")]
    [SerializeField] Image directionImage;
    [SerializeField] Text lightAttack;
    [SerializeField] Text heavyAttack;
    [SerializeField] Text frames;

    [Header("Direction Images")]
    [SerializeField] Color nullColor;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;
    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite upLeft;
    [SerializeField] Sprite upRight;
    [SerializeField] Sprite downLeft;
    [SerializeField] Sprite downRight;

    public void Assign(InputKey key)
    {
        DirectionImageSet(key);

        if (key.lKey == HitKey.Light)
            lightAttack.text = "A";
        else
            lightAttack.text = "";

        if (key.hKey == HitKey.Heavy)
            heavyAttack.text = "B";
        else
            heavyAttack.text = "";
    }

    public void FrameUpdate(int frameCount)
    {
        frames.text = frameCount.ToString();
    }

    void DirectionImageSet(InputKey direction)
    {
        directionImage.color = Color.white;
        switch(direction.dirKey)
        {
            case DirectionKey.Left:
                directionImage.sprite = left;
                break;
            case DirectionKey.Right:
                directionImage.sprite = right;
                break;
            case DirectionKey.Up:
                directionImage.sprite = up;
                break;
            case DirectionKey.Down:
                directionImage.sprite = down;
                break;
            case DirectionKey.UpLeft:
                directionImage.sprite = upLeft;
                break;
            case DirectionKey.UpRight:
                directionImage.sprite = upRight;
                break;
            case DirectionKey.DownLeft:
                directionImage.sprite = downLeft;
                break;
            case DirectionKey.DownRight:
                directionImage.sprite = downRight;
                break;
            default:
                directionImage.color = nullColor;
                break;
        }
    }
}
