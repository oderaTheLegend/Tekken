using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMaster : MonoBehaviour
{
    public static AnimationMaster instance = null;

    [SerializeField] uint framesPerSecond;
    [SerializeField] uint resetValue = 1;

    [SerializeField] float freezeFrameDuration = 0.5f;

    [SerializeField] float gravity = 1f;

    float universalTimer = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public float AnimFrames
    {
        get { return framesPerSecond; }
    }

    public float Freeze
    {
        get { return freezeFrameDuration; }
    }

    public float Gravity
    {
        get { return gravity; }
    }
}
