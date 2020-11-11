using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrameState
{
    Running,
    Finished,
    Looping
}

public class SpriteController : Controller
{
    //[Header("Animator Variables")]
    SpriteRenderer renderer;

    [Header("Neutral States")]
    [SerializeField] State idle;
    [SerializeField] State forward;
    [SerializeField] State jump;
    [SerializeField] State dirJump;

    [Header("Action States")]
    [SerializeField] float comboSetTime = 0.2f;
    [SerializeField] List<State> moveSets;

    Vector3 facing;
    State current;
    State buffer = null;
    float bufferTimer = 0;

    bool combo;
    bool changeState = true;

    protected override void Start()
    {
        base.Start();

        renderer = GetComponent<SpriteRenderer>();
        Current = idle;
        facing = transform.forward;
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
            if (changeState)
            {
                State temp = ComboCheck(inputs, frames);

                if (temp != null)
                {
                    buffer = temp;
                    bufferTimer = 0;
                }

                if (buffer == current)
                    buffer = null;

                if (buffer != null)
                    combo = true;
                else
                {
                    bufferTimer = 0;
                }
            }

            if (current == idle || current == forward)
            {
                combo = false;
                // Otherwise executes neutral/jump states
                if (inputs[1].jKey == HitKey.Jump && jumpState == JumpState.Grounded)
                {
                    if (current == idle)
                        current = jump;
                    if (current == forward)
                        current = dirJump;

                    current.Reset();
                    Jump();
                }
            }
        }

        float recTime;
        FrameState stateCheck = current.Animate(renderer, out recTime);

        bufferTimer += Time.deltaTime;

        if (stateCheck != FrameState.Running)
        {
            if (buffer != null && bufferTimer <= comboSetTime)
            {
                current = buffer;
                current.Reset();
                Debug.Log("Current combo : " + Current.name);
                combo = true;

                buffer = null;
                bufferTimer = 0;
            }
            else if (stateCheck == FrameState.Finished)
            {
                StartCoroutine(RecoveryWaiter(recTime));
                Current = idle;
            }
        }

        if ((current == jump || current == dirJump) && jumpState == JumpState.Grounded)
        {
            Current = idle;
        }
    }

    void FixedUpdateMovement()
    {
        Vector3 dir = InputManager.instance.Direction();

        if (!combo)
        {
            if (dir.z > 0)
                transform.forward = facing;
            else if (dir.z < 0)
                transform.forward = -facing;
        }

        bool move = false;

        if (!combo && Current.moveAllowed)
            Move(dir, out move);

        if (jumpState == JumpState.Grounded)
        {
            if (move)
                Current = forward;
            else
            {
                if (Current == forward)
                    current = idle;
            }
        }

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

    State ComboCheck(List<InputKey> input, List<int> frame)
    {
        int frameGap = InputManager.instance.FrameGap();

        for (int i = 0; i < moveSets.Count; i++)
        {
            bool check = true;
            for (int j = 0; j < moveSets[i].inputKey.Length; j++)
            {
                if (!moveSets[i].inputKey[j].Compare(input[j + 1]) || frame[j] > frameGap)
                {
                    check = false;
                    break;
                }
            }

            if (check)
            {
                if (moveSets[i].preReqState == null)
                {
                    if (current == idle || current == forward)
                        return moveSets[i];
                }
                else if (current == moveSets[i].preReqState)
                {
                    return moveSets[i];
                }
            }
        }

        return null;
    }

    State Current
    {
        get { return current; }
        set { if (current != value) { current = value; current.Reset(); } }
    }

    IEnumerator RecoveryWaiter(float t)
    {
        changeState = false;
        yield return new WaitForSeconds(t);
        changeState = true;
    }
}