using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //==========================================Variable==========================================
    [Header("===Game Manager===")]
    [Header("Level")]
    [SerializeField] private GameState level;
    [SerializeField] private List<Transform> maps;

    //===========================================Method===========================================
    public void NextLevel()
    {
        if (this.level == GameState.LEVEL_3) return;
        this.level++;
        this.ChangeLevel(this.level);
    }

    private void ChangeLevel(GameState level)
    {
        this.level = level;
    }
}
