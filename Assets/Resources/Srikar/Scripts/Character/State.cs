using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObjects/State", order = 1)]
public class State : ScriptableObject
{
    [SerializeField] public Sprite[] sprites;
    [SerializeField] bool loop;

    [Header("Frame Details")]
    [SerializeField] uint keyFrame;
    [SerializeField] uint framesTillKey;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 0;
    [SerializeField] public float vertSpeed = 0;

    [Header("Input")]
    [SerializeField] public InputKey[] inputKey;

    int index = 0;
    float time = 0;

    FrameState state;

    bool vertApplied = false;

    public FrameState Animate(Character chara, out float horizontal)
    {
        state = FrameState.Running;
        time += Time.deltaTime * AnimationMaster.instance.AnimFrames;

        //if (!vertApplied)
        //{
        //    float gravity = 2 * chara.jumpHeight / (sprites.Length * AnimationMaster.instance.AnimFrames * sprites.Length * AnimationMaster.instance.AnimFrames);
        //    chara.rigidbody.gravityScale = gravity;
        //
        //    chara.rigidbody.AddForce(2 * gravity * Vector3.up, ForceMode2D.Impulse);
        //    vertApplied = true;
        //}

        if (time > 1)
        {
            index += 1;

            if (index == keyFrame + 1 && !loop)
                state = FrameState.Cancelled;

            if (index >= sprites.Length)
            {
                index = 0;
                vertApplied = false;

                if (!loop)
                    state = FrameState.Finished;
            }

            chara.renderer.sprite = sprites[index];
            time = 0;
        }

        horizontal = moveSpeed * Time.deltaTime;

        return state;
    }

    public void Reset()
    {
        time = 0;
    }
}
