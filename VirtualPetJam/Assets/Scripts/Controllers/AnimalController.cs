using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [Header("Instances")]
    private GameManager gameManager;
    private LevelManager levelManager;

    [Header("Status")]
    public int animalID;
    public string animalName;
    public int fedLevel;
    public int cleanessLevel;
    public int pleasureLevel;

    void Start()
    {
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
    }

    void Update()
    {
        
    }

    public string GetAnimalName() { return animalName; }

    public int GetFedLevel() { return fedLevel; }

    public int GetCleanessLevel() { return cleanessLevel; }

    public int GetPleasureLevel() { return pleasureLevel; }

    public void SetAnimalID(int animalID) { this.animalID = animalID; }

    public void OnRenameAnimalBtn(string newName) //UI button to rename animal
    {
        levelManager.ChangeAnimalName(animalID, newName);
    }
}
