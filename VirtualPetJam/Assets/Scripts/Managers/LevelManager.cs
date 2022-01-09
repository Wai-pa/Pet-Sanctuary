using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Instances")]
    private GameManager gameManager;
    private SoundManager soundManager;
    public static LevelManager instance = null;

    public string playerName;
    public int food;
    public int soaps;
    public int toys;
    public int money;

    public struct Animals
    {
        public int animalID;
        public string animalName;
        public int fedLevel;
        public int cleanessLevel;
        public int pleasureLevel;
    }

    public List<Animals> listOfAnimals = new List<Animals>();

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        gameManager = GameManager.instance;
        soundManager = SoundManager.instance;
    }

    public void OnSaveLevel()
    {
        SaveSystem.SaveLevel();
    }

    public void OnLoadLevel()
    {
        SaveSystem.LoadLevel();
    }

    public void CreateAnimal(AnimalController controller)
    {
        Animals animal = new Animals();

        animal.animalID = listOfAnimals.Count + 1;
        controller.GetAnimalID(animal.animalID);
        animal.animalName = controller.SetAnimalName();
        animal.fedLevel = controller.SetFedLevel();
        animal.cleanessLevel = controller.SetCleanessLevel();
        animal.pleasureLevel = controller.SetPleasureLevel();

        listOfAnimals.Add(animal);
    }

    public void ChangeAnimalName(int animalID, string newName)
    {
        for(int i = 0; i < listOfAnimals.Count; i++)
        {
            if(listOfAnimals[i].animalID == animalID)
            {
                Animals animal = listOfAnimals[i];
                animal.animalName = newName;
                listOfAnimals[i] = animal;
                break;
            }
        }
    }
}
