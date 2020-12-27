using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class MenuOutlineEffect : MonoBehaviour
{
    [SerializeField] Material effectMat;

    public float scale = 1;

    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        effectMat.SetFloat("_Scale", scale);
        Graphics.Blit(source, dest, effectMat);
    }
}
