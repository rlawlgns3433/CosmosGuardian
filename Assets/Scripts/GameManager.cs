using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerStats playerStats = null;
    public bool IsGameover { get; set; }

    public void Gameover()
    {
        playerStats.stats[CharacterColumn.Stat.SpeedHorizontal] = 0;
        playerStats.stats[CharacterColumn.Stat.SpeedVertical] = 0;
    }
}
