using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [Header("Instances")]
    private GameManager gameManager;
    private LevelManager levelManager;

    [Header("Status")]
    [SerializeField] private int animalID;
    [SerializeField] private string animalName;
    [SerializeField] private int fedLevel;
    [SerializeField] private int cleanessLevel;
    [SerializeField] private int pleasureLevel;

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

    public void SetFedLevel(bool increase) { fedLevel = increase ? fedLevel += 1 : fedLevel -= 1; }

    public int GetCleanessLevel() { return cleanessLevel; }

    public void SetCleanessLevel(bool increase) { cleanessLevel = increase ? cleanessLevel += 1 : cleanessLevel -= 1; }

    public int GetPleasureLevel() { return pleasureLevel; }

    public void SetPleasureLevel(bool increase) { pleasureLevel = increase ? pleasureLevel += 1 : pleasureLevel -= 1; }

    public int GetAnimalID() { return animalID; }

    public void SetAnimalID(int animalID) { this.animalID = animalID; }
}
