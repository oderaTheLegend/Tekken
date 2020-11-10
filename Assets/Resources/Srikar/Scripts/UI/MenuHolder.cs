using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MenuHolder : MonoBehaviour
{
    CanvasGroup group;

    [Header("Buttons")]
    [SerializeField] List<Button> bList;

    [Header("Sub-Canvas Groups")]
    [SerializeField] List<CanvasGroup> cList;

    public void Prep()
    {
        foreach(var bttn in bList)
        {
            bttn.interactable = false;
            bttn.gameObject.SetActive(false);
        }

        foreach(var grp in cList)
        {
            grp.alpha = 0;
        }
    }

    public void ButtonActivate()
    {
        foreach (var bttn in bList)
        {
            bttn.interactable = true;
        }
    }

    public void ButtonDeactivate()
    {
        foreach (var bttn in bList)
        {
            bttn.interactable = false;
        }
    }

    public CanvasGroup Group
    { 
        get 
        {
            if (group != null)
                return group;
            else
            {
                group = group = GetComponent<CanvasGroup>();
                return group;
            }
        } 
    }

}