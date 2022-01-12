using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void CreateAnimal(AnimalController controller, string animalName, int animalID)
    {
        Animals animal = new Animals();

        controller.SetAnimalID(animalID);
        animal.animalName = animalName;
        animal.fedLevel = controller.GetFedLevel();
        animal.cleanessLevel = controller.GetCleanessLevel();
        animal.pleasureLevel = controller.GetPleasureLevel();

        listOfAnimals.Add(animal);
    }

    public void UpdateFood(bool increase) { food = increase ? food += 1 : food -= 1; }

    public void UpdateSoaps(bool increase) { soaps = increase ? soaps += 1 : soaps -= 1; }

    public void UpdateToys(bool increase) { toys = increase ? toys += 1 : toys -= 1; }

    public void UpdateMoney(bool increase) { money = increase ? money += 1 : money -= 1; }

    public void UpdateWood(bool increase) { wood = increase ? wood += 1 : wood -= 1; }

    void WoodSpawner()
    {
        int rnd = Random.Range(0, listOfTransformToSpawnWood.Count);
        Instantiate(woodGameObj, listOfTransformToSpawnWood[rnd].position, Quaternion.identity);
    }
}
