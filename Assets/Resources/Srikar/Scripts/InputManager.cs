using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DirectionKey
{
    Null,
    Up,
    Down,
    Left,
    Right,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft,
    Any
}

public enum HitKey
{
    Null,
    Light,
    Heavy,
    Jump
}

[Serializable]
public class InputKey
{
    public DirectionKey dirKey;
    public HitKey lKey;
    public HitKey hKey;
    public HitKey jKey;

    public InputKey()
    {
        dirKey = DirectionKey.Null;
        lKey = HitKey.Null;
        hKey = HitKey.Null;
        jKey = HitKey.Null;
    }

    public bool Compare(InputKey key)
    {
        if (key.dirKey == DirectionKey.Any || dirKey == DirectionKey.Any)
        {
            if (key.lKey == lKey && key.hKey == hKey && key.jKey == jKey)
                return true;
            else
                return false;
        }
        else if (key.dirKey == dirKey && key.lKey == lKey && key.hKey == hKey && key.jKey == jKey)
            return true;
        else
            return false;
    }
}

public struct PlayerInput
{
    InputKey[] inputs;
}

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    [Header("Input Frame Settings")]
    [SerializeField] int frameRate = 60;
    [SerializeField] int inputFrameLimit = 20;
    [SerializeField] int inputFrameGap = 10;

    [Header("UI Handlers")]
    [SerializeField] UIInputHistory historyUI;

    float horizontal;
    float vertical;
    float lightAttack;
    float heavyAttack;
    float jump;

    List<InputKey> inputHistory;

    List<int> frameHistory;
    bool currentInput;
    float time = 0;
    bool takeInput = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        inputHistory = new List<InputKey>();
        frameHistory = new List<int>();

        // First input tracker registered
        inputHistory.Add(new InputKey());
        frameHistory.Add(0);
        historyUI.AddInput(inputHistory[0]);
        currentInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        lightAttack = Input.GetAxisRaw("Fire1");
        heavyAttack = Input.GetAxisRaw("Fire2");
        jump = Input.GetAxisRaw("Jump");

        FightingInput();
    }

    void FightingInput()
    {
        time += Time.deltaTime * frameRate;

        if (time > 1)
        {
            frameHistory[0] += 1;
            time = 0;
        }
        historyUI.UpdateCurrentFrames(frameHistory[0]);

        if (takeInput)
        {
            // Direction Button check
            if (inputHistory[0].dirKey == DirectionKey.Null)
            {
                if (horizontal > 0)
                    inputHistory[0].dirKey = DirectionKey.Right;
                else if (horizontal < 0)
                    inputHistory[0].dirKey = DirectionKey.Left;

                if (vertical > 0)
                {
                    switch (inputHistory[0].dirKey)
                    {
                        case DirectionKey.Null:
                            inputHistory[0].dirKey = DirectionKey.Up;
                            break;
                        case DirectionKey.Right:
                            inputHistory[0].dirKey = DirectionKey.UpRight;
                            break;
                        case DirectionKey.Left:
                            inputHistory[0].dirKey = DirectionKey.UpLeft;
                            break;
                        default:
                            break;
                    }
                }
                else if (vertical < 0)
                {
                    switch (inputHistory[0].dirKey)
                    {
                        case DirectionKey.Null:
                            inputHistory[0].dirKey = DirectionKey.Down;
                            break;
                        case DirectionKey.Right:
                            inputHistory[0].dirKey = DirectionKey.DownRight;
                            break;
                        case DirectionKey.Left:
                            inputHistory[0].dirKey = DirectionKey.DownLeft;
                            break;
                        default:
                            break;
                    }
                }

                if (inputHistory[0].dirKey != DirectionKey.Null)
                    currentInput = true;
            }

            // Light Button Check
            if (inputHistory[0].lKey == HitKey.Null)
            {
                if (lightAttack > 0)
                {
                    inputHistory[0].lKey = HitKey.Light;
                    currentInput = true;
                }
            }

            // Heavy Button Check
            if (inputHistory[0].hKey == HitKey.Null)
            {
                if (heavyAttack > 0)
                {
                    inputHistory[0].hKey = HitKey.Heavy;
                    currentInput = true;
                }
            }

            // Jump Button Check
            if (inputHistory[0].jKey == HitKey.Null)
            {
                if (jump > 0)
                {
                    inputHistory[0].jKey = HitKey.Jump;
                    currentInput = true;
                }
            }
        }

        if (currentInput)
        {
            if (inputHistory.Count > 1)
            {
                if (!inputHistory[0].Compare(inputHistory[1]))
                {
                    historyUI.AddInput(inputHistory[0]);

                    inputHistory.Insert(0, new InputKey());

                    frameHistory.Insert(0, 0);
                    currentInput = false;
                }
                //else if (frameHistory[0] >= inputFrameGap)
                //{
                //    inputHistory.Insert(0, new InputKey());
                //
                //    frameHistory.Insert(0, 0);
                //    //frameHistory[0] += frameHistory[1];
                //
                //    //currentInput = false;
                //}
                else
                {
                    inputHistory[0] = new InputKey();
                }
            }
            else // Happens only once
            {
                historyUI.AddInput(inputHistory[0]);
                inputHistory.Insert(0, new InputKey());
                frameHistory.Insert(0, 0);
                currentInput = false;
            }
        }
        else
        {
            if (frameHistory[0] > inputFrameGap)
            {
                if (!inputHistory[0].Compare(new InputKey()))
                {
                    historyUI.AddInput(inputHistory[0]);
                    inputHistory.Insert(0, new InputKey());
                    frameHistory.Insert(0, 0);
                    currentInput = false;
                }
            }
        }

        // Removing input history at the end of the list
        if (frameHistory.Count > inputFrameLimit)
        {
            frameHistory.RemoveAt(frameHistory.Count - 1);
            inputHistory.RemoveAt(inputHistory.Count - 1);
        }
    }

    public List<InputKey> Inputs()
    {
        return inputHistory;
    }

    public List<int> Frames()
    {
        return frameHistory;
    }

    public int FrameGap()
    {
        return inputFrameGap;
    }

    public Vector3 Direction()
    {
        return new Vector3(vertical, 0, horizontal);
    }

    public int ReturnCurrentFrames()
    {
        return frameHistory[0];
    }

    public bool TakeInput
    {
        get { return takeInput; }
        set { takeInput = value; }
    }

    public void AddBlankInput()
    {
        historyUI.AddInput(inputHistory[0]);
        inputHistory.Insert(0, new InputKey());
        frameHistory.Insert(0, 0);
        currentInput = false;
    }
}