using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PushDown : MonoBehaviour
{
    public static PushDown instance = null;

    [Header("MenuHolders")]
    [SerializeField] public MenuHolder startMenu;
    [SerializeField] public MenuHolder mainMenu;
    [SerializeField] public MenuHolder lobbyMenu;
    [SerializeField] public MenuHolder characterMenu;

    [Header("Miscellaneous Menu's")]
    [SerializeField] PopUpWindow popUp;

    static Stack<GameState> stack;

    bool busy;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        Init();

        stack = new Stack<GameState>();

        stack.Push(new StartMenu(startMenu));
        stack.Peek().OpenState();
        busy = false;
    }

    private void Init()
    {
        startMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);     
        //lobbyMenu.gameObject.SetActive(false);
        //characterMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    { stack.Peek().UpdateState(); }

    public void Push(GameState state)
    { if (!busy) StartCoroutine(PushRoutine(state)); }

    public void Pop()
    { if (!busy) StartCoroutine(PopRoutine()); }

    public void PopUpBack(string str)
    { if (!busy) StartCoroutine(PopUpBackRoutine(str)); }

    IEnumerator PushRoutine(GameState state)
    {
        busy = true;
        stack.Peek().CloseState();

        while (stack.Peek().Loaded != false)
            yield return null;

        stack.Peek().End();

        stack.Push(state);
        stack.Peek().OpenState();
        busy = false;
    }

    IEnumerator PopRoutine()
    {
        busy = true;
        stack.Peek().CloseState();

        while (stack.Peek().Loaded != false)
            yield return null;

        stack.Peek().End();

        stack.Pop();

        if (stack.Count == 0)
            stack.Push(new StartMenu(startMenu));

        stack.Peek().OpenState();
        busy = false;
    }

    IEnumerator PopUpBackRoutine(string str)
    {
        busy = true;

        popUp.gameObject.SetActive(true);
        popUp.OpenWindow(str, PopUpType.CancelContinue);

        while (popUp.WindowReply == Reply.Waiting)
            yield return null;

        popUp.CloseWindow();

        if (popUp.WindowReply == Reply.Yes)
            Invoke("Pop", 0f);

        busy = false;
    }
}
