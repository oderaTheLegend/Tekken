using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public GameObject networkingPanel;
    public GameObject menu;

    public Button back;
    public Button joinGame;
    public Button login;

    private void Start()
    {
        networkingPanel.SetActive(false);
    }

    public void JoinGame()
    {
        Mode.mode = Mode.Modes.Online;
        StartCoroutine(EnableLoadingObj());
    }
    public void LoadCharacterSelect()
    {
        StartCoroutine(FadeToGame(1));
    }

    public void PressStart()
    {
        StartCoroutine(FadeToGame(1));
    }

    public void Back()
    {
       StartCoroutine(DisableLoadingObj());
    }

    IEnumerator EnableLoadingObj()
    {
        networkingPanel.SetActive(true); 
        back.interactable = false;
        login.interactable = false;
        joinGame.GetComponent<Button>().interactable = false;
        UIJuice.instance.GroupAlphaLerp(networkingPanel.GetComponent<CanvasGroup>(), 1, 1);
        UIJuice.instance.GroupAlphaLerp(menu.GetComponent<CanvasGroup>(), 0, 2);
        yield return new WaitForSeconds(2f);        
        back.interactable = true;
        login.interactable = true;
    }
    IEnumerator FadeToGame(int level)
    {
        Mode.mode = Mode.Modes.SinglePlayer;
        UIJuice.instance.GroupAlphaLerp(menu.GetComponent<CanvasGroup>(), 0, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(level);
    }
    IEnumerator DisableLoadingObj()
    {
        menu.SetActive(true);
        back.interactable = false;
        login.interactable = false;
        joinGame.interactable = false;
        UIJuice.instance.GroupAlphaLerp(networkingPanel.GetComponent<CanvasGroup>(), 0, 2);
        UIJuice.instance.GroupAlphaLerp(menu.GetComponent<CanvasGroup>(), 1, 1);
        yield return new WaitForSeconds(2f);
        networkingPanel.SetActive(false);
        joinGame.interactable = true;  

    }
}