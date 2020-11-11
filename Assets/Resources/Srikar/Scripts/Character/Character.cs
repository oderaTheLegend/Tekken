using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public enum FrameState
{
    Running,
    Finished,
    Cancelled
}

public class Character : MonoBehaviourPun
{
    [NonSerialized] public SpriteRenderer renderer;
    [NonSerialized] public Rigidbody2D rigidbody;

    [Header("Neutral States")]
    [SerializeField] State idle;
    [SerializeField] State forward;
    [SerializeField] State backward;
    [SerializeField] State jump;
    [SerializeField] State crouch;

    [Header("Action States")]
    [SerializeField] List<State> moveSets;

    State current;

    [Header("Platforming values")]
    [SerializeField] public float jumpHeight;
    [SerializeField] [Range(-1, 1)] int facing = 1;

    Vector3 rightFacing;

    bool grounded;

    public BoxCollider2D[] colliders;
    bool canJump; // delete later

    float lastMove;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        rightFacing = transform.forward;

        current = idle;
    }

    private void Update()
    {   
        if(Mode.mode == Mode.Modes.Online)
        {
            if (photonView.IsMine)
            {
                Movement();
            }
        }
        else
        {
            Movement();
        }              
    }

    void Movement()
    {
        List<InputKey> input = InputManager.instance.ReturnHistory();

        GroundCheck();

        if (input.Count > 1)
        {
            if (input[1].dirKey == DirectionKey.Right)
            {
                if (facing > 0)
                {
                    if (current != forward)
                        current = forward;
                }
                else
                {
                    if (current != backward)
                        current = backward;
                }
            }
            else if (input[1].dirKey == DirectionKey.Left)
            {
                if (facing > 0)
                {
                    if (current != backward)
                        current = backward;
                }
                else
                {
                    if (current != forward)
                        current = forward;
                }
            }
            else if (grounded)
            {
                if (current != idle)
                    current = idle;
            }

            if (input[1].dirKey == DirectionKey.Up)
            {
                if (current != jump)
                    current = jump;

                canJump = true;

                if (canJump)
                {
                    rigidbody.velocity = Vector2.up * jumpHeight;
                    canJump = false;
                }
            }
        }

        float move;
        if (current.Animate(this, out move) == FrameState.Finished)
            current = idle;

        if (facing > 0)
        {
            transform.forward = rightFacing;
        }
        else if (facing < 0)
        {
            transform.forward = -rightFacing;
        }

        if (current != jump)
            lastMove = move;

        transform.position += lastMove * facing * Vector3.right;
    }

    Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    void GroundCheck()
    {
        grounded = false;
        if (Physics2D.Raycast(transform.position, -Vector3.up, 0.01f))
        {
            grounded = true;
        }
    }
}