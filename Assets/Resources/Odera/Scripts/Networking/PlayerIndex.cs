using UnityEngine;

public class PlayerIndex : MonoBehaviour
{
    public static PlayerIndex i;

    public int playerIndex = 0;
    void Start()
    {
        if(i == null)
        {
            i = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}