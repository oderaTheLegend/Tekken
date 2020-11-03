using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputHistory : MonoBehaviour
{
    [SerializeField] UIInputKeys prefab;
    [SerializeField] int inputLimit = 20;

    UIInputKeys current;

    List<UIInputKeys> inputList;

    bool active;

    bool currentNull;

    private void Start()
    {
        active = true;
        inputList = new List<UIInputKeys>();
    }

    private void Update()
    {

    }

    public void AddInput(InputKey key)
    {
        if (active)
        {
            current = Instantiate(prefab, this.transform);
            current.Assign(key);
            inputList.Insert(0, current);
            current.transform.SetSiblingIndex(0);
        }

        if (inputList.Count > inputLimit)
        {
            var temp = inputList[inputList.Count - 1];
            inputList.Remove(temp);
            Destroy(temp.gameObject);
        }
    }

    public void UpdateCurrentFrames(int frameCount)
    {
        if (active && current != null)
            current.FrameUpdate(frameCount);
    }
}
