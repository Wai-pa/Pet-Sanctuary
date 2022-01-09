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

    public string SetAnimalName()
    {
        return animalName;
    }

    public int SetFedLevel()
    {
        return fedLevel;
    }

    public int SetCleanessLevel()
    {
        return cleanessLevel;
    }

    public int SetPleasureLevel()
    {
        return pleasureLevel;
    }

    public void GetAnimalID(int animalID)
    {
        this.animalID = animalID;
    }

    public void OnCreateBtn() //UI button to create a new animal
    {
        levelManager.CreateAnimal(this);
    }

    public void OnRenameAnimalBtn(string newName) //UI button to rename animal
    {
        levelManager.ChangeAnimalName(animalID, newName);
    }
}
