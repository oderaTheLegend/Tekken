using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : GameState
{
    public CharacterMenu(MenuHolder hold)
    { holder = hold; }

    public override void OpenState()
    {
        // Always put this before openstate to init the menuholder
        Start();

    }

    public override void CloseState()
    {
        // Always put this after closestate to prep end the menuholder
        End();
    }

    public override void UpdateState()
    {
        if (holder.Group.alpha == 1)
        {
            holder.ButtonActivate();
            loaded = true;
        }

        if (loaded)
        {
            // Functuionality here
        }
    }
}
