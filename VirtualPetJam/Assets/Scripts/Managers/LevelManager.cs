using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [Header("Instances")]
    public static LevelManager instance = null;
    [SerializeField] private CinemachineConfiner confiner;
    private GameManager gameManager;
    private SoundManager soundManager;

    [Header("Player Stats")]
    public string playerName;
    public int food;
    public int soaps;
    public int toys;
    public int money;
    public int rate;
    public bool isPlayerInTheFrontyard = false;
    public bool isPlayerInTheBackyard = false;

    [Header("Wood")]
    [SerializeField] private float timeToSpawnWood = 5f;
    private float spawnWoodTime;
    [SerializeField] private List<Vector3> listOfTransformToSpawnWood = new List<Vector3>();
    [SerializeField] private List<Vector3> listOfTransformOfSpawnedWoods = new List<Vector3>();
    [SerializeField] private GameObject woodGameObj;
    public int wood;

    [Header("Resources")]
    [SerializeField] private AnimalController animalSelectedController = null;
    [SerializeField] private List<Animals> listOfAnimals = new List<Animals>();
    [SerializeField] private List<GameObject> listOfAnimalsPrefabs = new List<GameObject>();
    [SerializeField] private PolygonCollider2D outsideCollider;
    [SerializeField] private PolygonCollider2D buildingCollider;

    [SerializeField] private List<Vector3> listOfAnimalSpawnPoints = new List<Vector3>();
    [SerializeField] private int animalSpawnPointInt;

    public struct Animals
    {
        public int animalID;
        public string animalName;
        public int fedLevel;
        public int cleanessLevel;
        public int pleasureLevel;
        public int overallLevel;
        public int spawnPosX;
        public int spawnPosY;
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
        InitializeAnimalSpawnPoints();
        InitializeWoodSpawnPositions();
        IsPlayerInTheFrontyard(false);
        IsPlayerInTheBackyard(false);
    }

    void Update()
    {
        if (isPlayerInTheFrontyard)
        {
            if (spawnWoodTime <= 0)
            {
                SpawnWood();
                spawnWoodTime = timeToSpawnWood;
            }
            else
            {
                spawnWoodTime -= Time.deltaTime;
            }
        }

        if(!isPlayerInTheBackyard && !isPlayerInTheFrontyard) { confiner.m_BoundingShape2D = buildingCollider; }
        else { confiner.m_BoundingShape2D = outsideCollider; }
    }

    public void OnSaveLevel() //UI button to save game
    {
        SaveSystem.SaveLevel();
    }

    public void OnLoadLevel() //UI button to load game
    {
        SaveSystem.LoadLevel();
    }

    public void CreateAnimal(string animalName, int animalSelected)
    {
        animalSpawnPointInt = Random.Range(0, listOfAnimalSpawnPoints.Count);
        Instantiate(listOfAnimalsPrefabs[animalSelected], listOfAnimalSpawnPoints[animalSpawnPointInt], Quaternion.identity);
        GameObject animalObj = GameObject.Find("Animal" + (animalSelected + 1) + "(Clone)");
        animalObj.name = animalName;
        
        animalSelectedController = animalObj.GetComponent<AnimalController>();
        animalSelectedController.SetAnimalID(animalSelected + 1);
        animalSelectedController.SetAnimalName(animalName);
        
        Animals animal = new Animals();

        animal.animalID = animalSelectedController.GetAnimalID();
        animal.animalName = animalName;
        animal.fedLevel = animalSelectedController.GetFedLevel();
        animal.cleanessLevel = animalSelectedController.GetCleanessLevel();
        animal.pleasureLevel = animalSelectedController.GetPleasureLevel();
        animal.overallLevel = animalSelectedController.GetOverallLevel();

        listOfAnimals.Add(animal);
        listOfAnimalSpawnPoints.Remove(listOfAnimalSpawnPoints[animalSpawnPointInt]);
    }

    public void UpdateFood(bool increase) { food = increase ? food += 1 : food -= 1; }

    public void UpdateSoaps(bool increase) { soaps = increase ? soaps += 1 : soaps -= 1; }

    public void UpdateToys(bool increase) { toys = increase ? toys += 1 : toys -= 1; }

    public void UpdateMoney(bool increase) { money = increase ? money += 1 : money -= 1; }

    public void UpdateWood(bool increase) { wood = increase ? wood += 1 : wood -= 1; }

    public void UpdateRate()
    {
        rate = 0;

        for (int i = 0; i < listOfAnimals.Count; i++)
        {
            rate += listOfAnimals[i].overallLevel;
        }
    }

    void InitializeWoodSpawnPositions()
    {
        listOfTransformToSpawnWood.Add(new Vector3(-3, -3, 0));
        listOfTransformToSpawnWood.Add(new Vector3(-7, -1, 0));
        listOfTransformToSpawnWood.Add(new Vector3(-7, -3, 0));
        listOfTransformToSpawnWood.Add(new Vector3(-7, -6, 0));
        listOfTransformToSpawnWood.Add(new Vector3(-4, -6, 0));
        listOfTransformToSpawnWood.Add(new Vector3(1, -4, 0));
        listOfTransformToSpawnWood.Add(new Vector3(12, -2, 0));
        listOfTransformToSpawnWood.Add(new Vector3(11, -5, 0));
        listOfTransformToSpawnWood.Add(new Vector3(6, -6, 0));
    }

    private void SpawnWood()
    {
        int rnd = Random.Range(0, listOfTransformToSpawnWood.Count);
        listOfTransformOfSpawnedWoods.Add(listOfTransformToSpawnWood[rnd]);
        Instantiate(woodGameObj, listOfTransformToSpawnWood[rnd], Quaternion.identity);
        listOfTransformToSpawnWood.Remove(listOfTransformToSpawnWood[rnd]);
    }

    public void RestoreSpawnedWoodPosition(Vector3 position)
    {
        listOfTransformToSpawnWood.Add(position);
        listOfTransformOfSpawnedWoods.Remove(position);
    }

    public void IsPlayerInTheFrontyard(bool yes) { isPlayerInTheFrontyard = yes; }

    public void IsPlayerInTheBackyard(bool yes) { isPlayerInTheBackyard = yes; }

    void InitializeAnimalSpawnPoints()
    {
        listOfAnimalSpawnPoints.Add(new Vector3(-7.5f, 1.7f, 0));
        listOfAnimalSpawnPoints.Add(new Vector3(-7.5f, 8.7f, 0));
        listOfAnimalSpawnPoints.Add(new Vector3(-7.5f, 12.3f, 0));
        listOfAnimalSpawnPoints.Add(new Vector3(-1f, 8.7f, 0));
        listOfAnimalSpawnPoints.Add(new Vector3(6f, 5.5f, 0));
        listOfAnimalSpawnPoints.Add(new Vector3(12f, 12.8f, 0));
    }

    public int GetAnimalSpawnPointInt() { return animalSpawnPointInt; }

    public int GetSizeOfAnimalSpawnPointList() { return listOfAnimalSpawnPoints.Count; }
}
