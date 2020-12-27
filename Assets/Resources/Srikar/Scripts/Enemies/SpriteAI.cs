using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    Walking,
    Strafing,
    Attacking,
    Retreating,
    Hit
}

public class SpriteAI : Controller
{
    Animator anim;

    Vector3 facing;

    InputState inputState;

    AITarget target;

    AIState state;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        facing = transform.forward;
        inputState = InputState.Neutral;

        target = FindObjectOfType<AITarget>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFunction();
    }

    void FixedUpdate()
    {
        FixedUpdateFunction();
    }

    void UpdateFunction()
    {
        GroundCheck();

        //Jump here
    }

    void FixedUpdateFunction()
    {
        Vector3 dir = new Vector3();

        bool move = false;

        if (inputState == InputState.Neutral)
            Move(dir, out move);

        if (move)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        if (jumpState == JumpState.Grounded)
            anim.SetBool("Grounded", false);
        else
            anim.SetBool("Grounded", false);

        Gravity();
    }

    void AIFunction()
    {
        switch (state)
        {
            case AIState.Walking:
                break;
            case AIState.Strafing:
                break;
            case AIState.Retreating:
                break;
            case AIState.Attacking:
                break;
            case AIState.Hit:
                break;
            default:
                break;
        }
    }
}

// AI cycle
// 1) Move into position
// 2) Strafe around
// 3) Move in and attack
// 4) Retreat to position
// 5) Find new position to attack
