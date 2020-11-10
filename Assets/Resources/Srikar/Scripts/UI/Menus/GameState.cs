using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected MenuHolder holder;
    protected bool loaded;

    protected void Start()
    {
        holder.gameObject.SetActive(true);
        holder.Group.alpha = 0;
        loaded = false;

        UIJuice.instance.GroupAlphaLerp(holder.Group, 1);
    }

    public void End()
    {
        holder.Group.alpha = 0;
        holder.gameObject.SetActive(false);
        loaded = false;
    }

    public abstract void OpenState();

    public abstract void UpdateState();

    public abstract void CloseState();

    public bool Loaded
    { get { return loaded; } }
}
