using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string playerName;
    public int food;
    public int soaps;
    public int toys;
    public int money;

    public LevelData()
    {
        LevelManager levelManager = LevelManager.instance;
        playerName = levelManager.playerName;
        food = levelManager.food;
        soaps = levelManager.soaps;
        toys = levelManager.toys;
        money = levelManager.money;
    }
}
