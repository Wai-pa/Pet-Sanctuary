using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Instances")]
    private GameManager gameManager;
    private SoundManager soundManager;
    public static LevelManager instance = null;

    [Header("Level Stats")]
    public string playerName;
    public int food;
    public int soaps;
    public int toys;
    public int money;
    public int satisfaction;

    [Header("Resources")]
    public GameObject firstAnimalPrefab; //temporary
    public List<Animals> listOfAnimals = new List<Animals>();
    [SerializeField] private List<GameObject> listOfAnimalsPrefabs = new List<GameObject>();
    [SerializeField] private GameObject wood;

    public struct Animals
    {
        public int animalID;
        public string animalName;
        public int fedLevel;
        public int cleanessLevel;
        public int pleasureLevel;
    }

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

    public void OnSaveLevel() //UI button to save game
    {
        SaveSystem.SaveLevel();
    }

    public void OnLoadLevel() //UI button to load game
    {
        SaveSystem.LoadLevel();
    }

    public void OnCreateBtn() //UI button to create a new animal
    {
        Instantiate(firstAnimalPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        CreateAnimal(firstAnimalPrefab.GetComponent<AnimalController>());
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
