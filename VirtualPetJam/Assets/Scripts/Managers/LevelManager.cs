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
    public int rate;
    public bool isPlayerInTheFrontyard = false;
    public bool isPlayerInTheBackyard = false;

    [Header("Wood")]
    [SerializeField] private float timeToSpawnWood = 5f;
    [SerializeField] private List<Vector3> listOfTransformToSpawnWood = new List<Vector3>();
    [SerializeField] private List<Vector3> listOfTransformOfSpawnedWoods = new List<Vector3>();
    [SerializeField] private GameObject woodGameObj;
    public int wood;

    [Header("Resources")]
    [SerializeField] private AnimalController animalSelectedController = null;
    [SerializeField] private List<Animals> listOfAnimals = new List<Animals>();
    [SerializeField] private List<GameObject> listOfAnimalsPrefabs = new List<GameObject>();

    public struct Animals
    {
        public int animalID;
        public string animalName;
        public int fedLevel;
        public int cleanessLevel;
        public int pleasureLevel;
        public int overallLevel;
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
        IsPlayerInTheFrontyard(false);
        IsPlayerInTheBackyard(false);
        StartCoroutine(SpawnWoods());
    }

    void Update()
    {

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
        GameObject animalObj = listOfAnimalsPrefabs[animalSelected];
        animalSelectedController = animalObj.GetComponent<AnimalController>();
        Instantiate(animalObj, new Vector3(0, 0, 0), Quaternion.identity);

        Animals animal = new Animals();

        animalSelectedController.SetAnimalID(animalSelected);
        animal.animalName = animalName;
        animal.fedLevel = animalSelectedController.GetFedLevel();
        animal.cleanessLevel = animalSelectedController.GetCleanessLevel();
        animal.pleasureLevel = animalSelectedController.GetPleasureLevel();

        listOfAnimals.Add(animal);
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

    IEnumerator SpawnWoods()
    {
        InitializeWoodSpawnPositions();

        while (true)
        {
            if (isPlayerInTheFrontyard)
            {
                yield return new WaitForSeconds(timeToSpawnWood);

                if (listOfTransformToSpawnWood.Count > 0)
                {
                    int rnd = Random.Range(0, listOfTransformToSpawnWood.Count);
                    listOfTransformOfSpawnedWoods.Add(listOfTransformToSpawnWood[rnd]);
                    Instantiate(woodGameObj, listOfTransformToSpawnWood[rnd], Quaternion.identity);
                    listOfTransformToSpawnWood.Remove(listOfTransformToSpawnWood[rnd]);
                }
            }
        }
    }

    public void RestoreSpawnedWoodPosition(Vector3 position)
    {
        listOfTransformToSpawnWood.Add(position);
        listOfTransformOfSpawnedWoods.Remove(position);
    }

    public void IsPlayerInTheFrontyard(bool yes) { isPlayerInTheFrontyard = yes ? true : false; }

    public void IsPlayerInTheBackyard(bool yes) { isPlayerInTheBackyard = yes ? true : false; }
}
