using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType
{
    Null,
    Continue,
    CancelContinue
}

public enum Reply
{
    Waiting,
    Yes,
    No
}

[RequireComponent(typeof(CanvasGroup))]
public class PopUpWindow : MonoBehaviour
{
    CanvasGroup group;

    [SerializeField] float fadeDuration = 0.2f;
    [SerializeField] Text info;
    [SerializeField] CanvasGroup gaurder;

    [SerializeField] Button[] buttons;

    Reply reply;

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();

        EndWindow();
    }

    private void Update()
    {
        if (group.alpha == 0 && !active)
        {
            EndWindow();
        }
    }

    void EndWindow()
    {
        gaurder.alpha = 0;
        group.alpha = 0;
        gaurder.gameObject.SetActive(false);
        group.gameObject.SetActive(false);
    }

    void WindowType(PopUpType type)
    {
        switch (type)
        {
            case PopUpType.Continue:
                buttons[0].gameObject.SetActive(false);
                buttons[1].gameObject.SetActive(true);
                break;
            case PopUpType.CancelContinue:
                buttons[0].gameObject.SetActive(true);
                buttons[1].gameObject.SetActive(true);
                break;
            default:
                buttons[0].gameObject.SetActive(false);
                buttons[1].gameObject.SetActive(false);
                break;
        }
    }

    public void OpenWindow(string str, PopUpType type)
    {
        WindowType(type);
        reply = Reply.Waiting;
        info.text = str;
        group.gameObject.SetActive(true);
        gaurder.gameObject.SetActive(true);
        UIJuice.instance.GroupAlphaLerp(gaurder, 1, fadeDuration);
        UIJuice.instance.GroupAlphaLerp(group, 1, fadeDuration);
        buttons[0].interactable = true;
        buttons[1].interactable = true;
        active = true;
    }

    public void CloseWindow()
    {
        UIJuice.instance.GroupAlphaLerp(gaurder, 0, fadeDuration);
        UIJuice.instance.GroupAlphaLerp(group, 0, fadeDuration);
        buttons[0].interactable = false;
        buttons[1].interactable = false;
        active = false;
    }

    public void SelectLevel()
    {
        gaurder.gameObject.SetActive(true);
        UIJuice.instance.GroupAlphaLerp(gaurder, 1, fadeDuration);
    }

    public void Continue()
    {
        reply = Reply.Yes;
    }

    public void Cancel()
    {
        reply = Reply.No;
    }

    public Reply WindowReply
    { get { return reply; } }
}