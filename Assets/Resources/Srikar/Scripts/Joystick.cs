using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joyStickBlackPart;
    private Image joystick;

    [HideInInspector] public Vector3 inputDirection;

    [HideInInspector] public bool attack;
    [HideInInspector] public bool jump;

    [HideInInspector] public bool selected;
    [HideInInspector] public bool unselected;

    [HideInInspector] public bool right;
    [HideInInspector] public bool left;


    void Start()
    {
        joyStickBlackPart = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
        inputDirection = Vector3.zero;
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 position = Vector2.zero;

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joyStickBlackPart.rectTransform, data.position, data.pressEventCamera, out position);

        position.x /= joyStickBlackPart.rectTransform.sizeDelta.x;
        position.y /= joyStickBlackPart.rectTransform.sizeDelta.y;

        float x = (joyStickBlackPart.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        float y = (joyStickBlackPart.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        inputDirection = new Vector3(y, 0, x);
        inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection;

        //to define the area in which joystick can move around
        joystick.rectTransform.anchoredPosition = new Vector3(inputDirection.z * (joyStickBlackPart.rectTransform.sizeDelta.y / 3), inputDirection.x * (joyStickBlackPart.rectTransform.sizeDelta.x) / 3);

    }

    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        inputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void Attack()
    {
        attack = true;
        jump = false;
    }

    public void Jump()
    {
        attack = false;
        jump = true;
    }

    public void Selected()
    {
        unselected = false;
        selected = true;
    }

    public void UnSelected()
    {
        selected = false;
        unselected = true;
    }
    public void Right()
    {
        right = true;
    }

    public void Left()
    {
        left = true;
    }
}