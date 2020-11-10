using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InfoBox : MonoBehaviour
{
    CanvasGroup group;

    [SerializeField] Text info;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        CloseBox();
    }

    public void OpenBox(string str)
    {
        info.text = str;

        group.alpha = 1;
        group.gameObject.SetActive(true);
    }

    public void CloseBox()
    {
        group.alpha = 0;
        group.gameObject.SetActive(false);
    }
}