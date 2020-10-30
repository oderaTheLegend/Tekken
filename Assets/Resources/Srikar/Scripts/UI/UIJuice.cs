using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJuice : MonoBehaviour
{
    public static UIJuice instance = null;

    [Header("Default Tween Duration")]
    [SerializeField] float groupTime = 1f;
    [SerializeField] float buttonTime = 1f;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void GroupAlphaLerp(CanvasGroup group, float alpha, float time = -1)
    {
        if (time == -1)
        { time = groupTime; }

        StartCoroutine(GroupAlphaLerpRoutine(group, alpha, time));
    }

    IEnumerator GroupAlphaLerpRoutine(CanvasGroup group, float alpha, float time)
    {
        float temp = 0;
        float startAlpha = group.alpha;

        yield return null;

        while (temp <= 1.05f)
        {
            temp += Time.deltaTime / time;

            group.alpha = Mathf.Lerp(startAlpha, alpha, temp);

            yield return null;
        }
    }
}
