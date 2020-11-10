using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : Controller
{
    //[Header("Animator Variables")]
    SpriteRenderer renderer;

    [Header("Neutral States")]
    [SerializeField] State idle;
    [SerializeField] State forward;

    [Header("Jump States")]
    [SerializeField] State jump;
    [SerializeField] State fall;

    [Header("Action States")]
    [SerializeField] List<State> moveSets;

    Vector3 facing;
    State current;

    bool combo;

    protected override void Start()
    {
        base.Start();

        renderer = GetComponent<SpriteRenderer>();
        current = idle;
        facing = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        List<InputKey> inputs = InputManager.instance.Inputs();

        if (inputs.Count > 1)
        {
            // Checks for combo or action states
            if (ComboCheck(inputs))
            {
                combo = true;
                Debug.Log("Current combo : " + current.name);
            }
            else
            {
                combo = false;
                // Otherwise executes neutral/jump states
                if (inputs[1].jKey == HitKey.Jump)
                {
                    Jump();
                    if (current != jump)
                    {
                        current = jump;
                        current.Reset();
                    }
                }
            }
        }

        if (current.Animate(renderer) == FrameState.Finished && jumpState == JumpState.Grounded)
        {
            current = idle;
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = InputManager.instance.Direction();

        if (dir.z > 0)
            transform.forward = facing;
        else if (dir.z < 0)
            transform.forward = -facing;

        bool move = false;

        if (!combo)
            Move(dir, out move);

        if (jumpState == JumpState.Grounded)
        {
            if (move)
                current = forward;
            else
                current = idle;
        }

        Gravity();
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
                current = moveSets[i];
                current.Reset();
                return true;
            }
        }

        return false;
    }
}