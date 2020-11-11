using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    public const float enemyMinSpawnTime = 5.0f;
    public const float enemyMaxSpawnTime = 10.0f;
         
    public const float playerSpawnTime = 4.0f;
        
    public const int playerMaxLives = 3;      
    public const string playerLives = "PlayerLives";

    public const string playerReady = "IsPlayerReady";
    public const string playerLoaded = "PlayerLoadedLevel";
    public const string playerDisconnected = "PlayerDisconnected";

    public static InGame instance;

    private void Awake()
    {
        instance = this;
    }

    public static Color ColorOfTeam(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.red;
            case 1: return Color.white;
        }

        return Color.black;
    }
}