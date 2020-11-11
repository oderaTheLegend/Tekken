using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ColType
{
    Hit,
    Hurt
}

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObjects/State", order = 1)]
public class State : ScriptableObject
{
    [SerializeField] public Sprite[] sprites;

    [Header("Frame Details")]
    [SerializeField] bool loop;
    [SerializeField] uint keyFrame;
    [SerializeField] float recoveryPeriod;
    [SerializeField] bool hasHit;

    [Header("Input")]
    [SerializeField] public InputKey[] inputKey;

    [Header("Pre-Req")]
    [SerializeField] public bool moveAllowed;
    [SerializeField] public State preReqState;

    int index = 0;
    float time = 0;
    bool jump = false;
    bool finishCheck = false;

    [NonSerialized] public FrameState state;

    bool colliderHit = false;

    public FrameState Animate(SpriteRenderer renderer, out float recoveryTime)
    {
        time += Time.deltaTime * AnimationMaster.instance.AnimFrames;

        if (time > 1)
        {
            if (state != FrameState.Finished)
            {
                if (index == keyFrame + 1 && colliderHit && hasHit)
                {
                    index = (int)keyFrame;
                    colliderHit = false;
                }

                if (index >= sprites.Length)
                {
                    if (state != FrameState.Looping)
                    {
                        if (!finishCheck)
                            finishCheck = true;
                        else
                            state = FrameState.Finished;

                        index = sprites.Length - 1;
                    }
                    else
                    {
                        index = 0;
                    }
                }

                renderer.sprite = sprites[index];
                index += 1;

                time = 0;
            }
        }

        recoveryTime = recoveryPeriod;

        return state;
    }

    public void Reset()
    {
        time = 0;
        index = 0;
        jump = false;
        finishCheck = false;
        colliderHit = false;

        if (loop)
            state = FrameState.Looping;
        else
            state = FrameState.Running;
    }

    public void ColliderHit()
    {
        colliderHit = true;
    }

    public bool HasHitCollider
    {
        get { return hasHit; }
    }
}