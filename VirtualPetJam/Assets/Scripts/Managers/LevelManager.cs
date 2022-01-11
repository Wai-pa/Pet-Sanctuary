using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Instances")]
    private GameManager gameManager;
    private SoundManager soundManager;
    public static LevelManager instance = null;

    [Header("Player Stats")]
    public string playerName;
    public int food;
    public int soaps;
    public int toys;
    public int money;
    public int satisfaction;

    [Header("Level Stats")]
    public bool isPlayerInTheWoods = false;

    [Header("Resources")]
    [SerializeField] private GameObject firstAnimalPrefab; //temporary
    [SerializeField] private List<Animals> listOfAnimals = new List<Animals>();
    [SerializeField] private List<GameObject> listOfAnimalsPrefabs = new List<GameObject>();
    [SerializeField] private List<Transform> listOfTransformToSpawnWood = new List<Transform>();
    [SerializeField] private GameObject woodGameObj;
    public int wood;

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

    void Update()
    {
        if (isPlayerInTheWoods) { WoodSpawner(); }
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
        controller.SetAnimalID(animal.animalID);
        animal.animalName = controller.GetAnimalName();
        animal.fedLevel = controller.GetFedLevel();
        animal.cleanessLevel = controller.GetCleanessLevel();
        animal.pleasureLevel = controller.GetPleasureLevel();

        listOfAnimals.Add(animal);
        // need to instantiate the right animal
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

    public void UpdateFood(bool increase) { food = increase ? food++ : food--; }

    public void UpdateSoaps(bool increase) { soaps = increase ? soaps++ : soaps--; }

    public void UpdateToys(bool increase) { toys = increase ? toys++ : toys--; }

    public void UpdateMoney(bool increase) { money = increase ? money++ : money--; }

    public void UpdateWood(bool increase) { wood = increase ? wood++ : wood--; }

    void WoodSpawner()
    {
        int rnd = Random.Range(0, listOfTransformToSpawnWood.Count);
        Instantiate(woodGameObj, listOfTransformToSpawnWood[rnd].position, Quaternion.identity);
    }
}
