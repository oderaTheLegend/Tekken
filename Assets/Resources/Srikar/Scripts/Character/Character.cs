using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

public enum FrameState
{
    Running,
    Finished,
    Cancelled
}

public class Character : MonoBehaviour
{
    [NonSerialized] public SpriteRenderer renderer;
    [NonSerialized] public Rigidbody2D rigidbody;

    [SerializeField] State idle;

    [SerializeField] State forward;

    [SerializeField] State backward;

    [SerializeField] State jump;

    State current;

    [SerializeField] [Range(-1, 1)] int facing = 1;

    [Header("Platforming values")]
    [SerializeField] public float jumpHeight;

    Vector3 rightFacing;

    bool grounded;

    float lastMove;

    public BoxCollider2D[] colliders;

    bool canJump; // delete later

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        rightFacing = transform.forward;

        current = idle;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        GroundCheck();

        if (horizontal * facing > 0)
        {
            if (current != forward) { 
                current = forward;
            }
        }
        else if (horizontal * facing < 0)
        {
            if (current != backward) { 
                current = backward; 
            }
        }
        else if (grounded)
        {
            if (current != idle) { 
                current = idle;
            }
        }

        if (vertical > 0)
        {
            if (current != jump) { 
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