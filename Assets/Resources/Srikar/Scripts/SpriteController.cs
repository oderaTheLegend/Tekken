using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    Collider other;
    bool fall;

    bool canMove = true;

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
        bool move = false;

        if (canMove)
        {
            Vector3 dir = InputManager.instance.Direction();

            if (inputState == InputState.Neutral)
            {
                if (dir.z > 0)
                    transform.forward = facing;
                else if (dir.z < 0)
                    transform.forward = -facing;
            }

            if (inputState == InputState.Neutral)
                Move(dir, out move);

            if (move)
                anim.SetBool("Walk", true);
            else
                anim.SetBool("Walk", false);

        }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Punch"))
        {
            canMove = false;
            fall = false;
            this.other = other;
            anim.SetTrigger("Fall");
        }
    }

    void Fall()
    {
        Vector3 dir = transform.position - other.transform.position;

        if (!fall)
        {
            HealthAmount(5);
            transform.position += new Vector3(dir.x * 2.3f, 0, 0);
            fall = true;           
        }
    }

    void GetUp()
    {
        if (!canMove)
        {
            canMove = true;
        }
    }

    public void HealthAmount(int damage)
    {
        GameManager.i.p1Health.value -= damage;

        if (GameManager.i.p1Health.value <= 0)
        {
            anim.SetBool("Die", true);
            GameManager.i.p1Health.value = 0;
        }

        if (GameManager.i.p1Health.value >= 100)
        {
            GameManager.i.p1Health.value = 100;
        }
    }
}