using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    [NonSerialized] public SpriteRenderer renderer;
    [NonSerialized] public Rigidbody rigidbody;

    [Header("Neutral States")]
    [SerializeField] State idle;
    [SerializeField] State forward;
    [SerializeField] State jump;
    [SerializeField] State falling;

    [Header("Action States")]
    [SerializeField] List<State> moveSets;

    State current;

    [Header("Platforming values")]
    [SerializeField] public float jumpHeight;
    
    int horizontal = 0;
    int vertical = 0;

    Vector3 rightFacing;

    bool shouldCheckGround;
    bool grounded;

    bool canJump; // delete later

    float lastMove;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody>();

        rightFacing = transform.forward;

        current = idle;

        shouldCheckGround = true;
    }

    private void Update()
    {
        List<InputKey> input = InputManager.instance.Inputs();

        if (shouldCheckGround)
            GroundCheck();

        float temp;
        if (current.Animate(renderer, out temp) == FrameState.Finished && grounded)
        {
            current = idle;
            InputManager.instance.TakeInput = true;
            InputManager.instance.AddBlankInput();
        }

        if (input.Count > 1)
        {
            if (grounded)
                if (!ComboCheck(input))
                {
                    switch (input[1].dirKey)
                    {
                        case DirectionKey.Right:
                            horizontal = 1;
                            vertical = 0;
                            current = forward;
                            break;
                        case DirectionKey.UpRight:
                            horizontal = 1;
                            vertical = 1;
                            current = forward;
                            break;
                        case DirectionKey.DownRight:
                            horizontal = 1;
                            vertical = -1;
                            current = forward;
                            break;
                        case DirectionKey.Left:
                            horizontal = -1;
                            vertical = 0;
                            current = forward;
                            break;
                        case DirectionKey.UpLeft:
                            horizontal = -1;
                            vertical = 1;
                            current = forward;
                            break;
                        case DirectionKey.DownLeft:
                            horizontal = -1;
                            vertical = -1;
                            current = forward;
                            break;
                        case DirectionKey.Up:
                            horizontal = 0;
                            vertical = 1;
                            current = forward;
                            break;
                        case DirectionKey.Down:
                            horizontal = 0;
                            current = forward;
                            vertical = -1;
                            break;
                        default:
                            horizontal = 0;
                            vertical = 0;
                            current = idle;
                            break;
                    }
                }
        }

        //current.StateMove(this, horizontal, vertical);

        if (horizontal > 0)
        {
            transform.forward = rightFacing;
        }
        else if (horizontal < 0)
        {
            transform.forward = -rightFacing;
        }
    }

    bool ComboCheck(List<InputKey> input)
    {
        for (int i = 0; i < moveSets.Count; i++)
        {
            bool check = true;
            for (int j = 0; j < moveSets[i].inputKey.Length; j++)
            {
                if (!moveSets[i].inputKey[j].Compare(input[j + 1]))
                {
                    check = false;
                    break;
                }
            }

            if (check && current != moveSets[i])
            {
                Debug.Log(current.name);
                current = moveSets[i];
                current.Reset();
                return true;
            }
        }

        return false;
    }

    void GroundCheck()
    {
        grounded = false;
        if (Physics.Raycast(transform.position, -Vector3.up, 0.01037f))
        {
            grounded = true;
        }
    }

    public bool GroundCheckStall
    {
        get { return shouldCheckGround; }
        set { shouldCheckGround = value; }
    }
}