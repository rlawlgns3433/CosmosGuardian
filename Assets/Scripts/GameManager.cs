using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerStats playerStats = null;
    public bool IsGameover { get; set; }

    public void Gameover()
    {
        playerStats.speedHorizontal = 0;
        playerStats.speedVertical= 0;
    }
}
