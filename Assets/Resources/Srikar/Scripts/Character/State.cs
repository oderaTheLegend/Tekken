using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObjects/State", order = 1)]
public class State : ScriptableObject
{
    [SerializeField] public Sprite[] sprites;
    [SerializeField] public BoxCollider2D[] colliders;

    [SerializeField] bool loop;

    [Header("Frame Details")]
    [SerializeField] uint keyFrame;
    [SerializeField] uint framesTillKey;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 0;
    [SerializeField] public float jumpVal = 0;

    [Header("Set Axis Movement")]
    [SerializeField] bool enable = false;
    [SerializeField] [Range(-1, 1)] public float horizontal = 0;
    [SerializeField] [Range(-1, 1)] public float vertical = 0;

    [Header("Input")]
    [SerializeField] public bool inputOverride = false;
    [SerializeField] public InputKey[] inputKey;

    int index = 0;
    float time = 0;
    bool jump = false;

    FrameState state;

    bool colliderHit = false;

    public FrameState Animate(SpriteRenderer renderer)
    {
        time += Time.deltaTime * AnimationMaster.instance.AnimFrames;

        if (time > 1)
        {
            if (state == FrameState.Running)
            {
                index += 1;

                if (index == keyFrame && !loop && colliderHit)
                {
                    state = FrameState.Cancelled;
                    InputManager.instance.TakeInput = true;
                }

                if (index >= sprites.Length)
                {
                    if (!loop)
                    {
                        state = FrameState.Finished;
                        index = sprites.Length - 1;
                    }
                    else
                    {
                        index = 0;
                    }
                }

                renderer.sprite = sprites[index];

                time = 0;
            }
        }

        return state;
    }

    public void StateMove(Character chara, float hor, float vert)
    {
        if (!enable)
        {
            Vector3 temp = new Vector3(hor, 0, vert).normalized;

            chara.transform.position += temp * Time.deltaTime * moveSpeed;
        }
        else
        {
            Vector2 temp = new Vector2(horizontal, vertical).normalized;

            chara.transform.position += chara.transform.right * temp.x * Time.deltaTime * moveSpeed;
            chara.transform.position += chara.transform.forward * temp.y * Time.deltaTime * moveSpeed * hor;
        }
    }

    public void Reset()
    {
        time = 0;
        index = 0;
        jump = false;

        state = FrameState.Running;

        if (!inputOverride)
        {
            InputManager.instance.TakeInput = false;
        }
    }

    public void ColliderHit()
    {
        colliderHit = true;
    }
}
