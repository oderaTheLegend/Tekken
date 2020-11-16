using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum InputState
{
    Neutral,
    Combo,
    Recovery
}

public class SpriteController : Controller
{
    //[Header("Animator Variables")]
    SpriteRenderer renderer;
    Animator anim;

    [SerializeField] float comboSetTime = 0.2f;

    [Header("Specified Inputs")]
    [SerializeField] InputKey lightAttack; 

    Vector3 facing;

    InputState inputState;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        facing = transform.forward;
        inputState = InputState.Neutral;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            if (photonView.IsMine)
            {
                UpdateMovement();
            }
        }
        else
        {
            UpdateMovement();
        }

    }

    void UpdateMovement()
    {
        GroundCheck();

        List<InputKey> inputs = InputManager.instance.Inputs();
        List<int> frames = InputManager.instance.Frames();

        if (inputs.Count > 1)
        {
            // Checks for combo or action states
            if (inputState != InputState.Combo)
            {
                ComboCheck(inputs, frames);
            }

            if (inputState == InputState.Neutral)
            {
                // Otherwise executes neutral/jump states
                if (inputs[1].jKey == HitKey.Jump && jumpState == JumpState.Grounded)
                {
                    anim.SetTrigger("Jump");
                    Jump();
                }
            }
        }
    }


    void FixedUpdateMovement()
    {
        Vector3 dir = InputManager.instance.Direction();

        if (inputState == InputState.Neutral)
        {
            if (dir.z > 0)
                transform.forward = facing;
            else if (dir.z < 0)
                transform.forward = -facing;
        }

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

    private void FixedUpdate()
    {
        if (Mode.mode == Mode.Modes.Online)
        {
            if (photonView.IsMine)
            {
                FixedUpdateMovement();
            }
        }
        else
        {
            FixedUpdateMovement();
        }
    }

    void ComboCheck(List<InputKey> input, List<int> frames)
    {
        int frameGap = InputManager.instance.FrameGap();

        if (input[1].Compare(lightAttack))
        {
            if (inputState == InputState.Neutral)
            {
                inputState = InputState.Combo;
                anim.SetTrigger("Light");
            }
            else 
            { 

            }
        }
    }

    public void WaitResetInputState(float time)
    {
        StartCoroutine(RecoveryWaiter(time));
    }

    public void HardResetInputState()
    {
        inputState = InputState.Neutral;
    }

    IEnumerator RecoveryWaiter(float time)
    {
        inputState = InputState.Recovery;
        yield return new WaitForSeconds(time);
        inputState = InputState.Neutral;
    }
}