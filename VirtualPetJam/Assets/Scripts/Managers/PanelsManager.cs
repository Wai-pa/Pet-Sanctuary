using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{
    public static PanelsManager instance = null;
    private LevelManager levelManager;

    [Header("Resources")]
    [SerializeField] private List<GameObject> listOfAnimalsPrefabs = new List<GameObject>();
    [SerializeField] private List<Sprite> listOfAnimalsImages = new List<Sprite>();
    [SerializeField] private AnimalController animalSelectedController = null;

    [Header("Create Animal Panel")]
    [SerializeField] private GameObject createAnimalPanel;
    [SerializeField] private Sprite animalCreateImg;
    [SerializeField] private Text animalDescriptionTxt;
    [SerializeField] private string nameInput;
    [SerializeField] private int animalSelected;

    [Header("Animal Stats Panel")]
    [SerializeField] private GameObject animalStatsPanel;
    [SerializeField] private Sprite animalStatsImg;
    [SerializeField] private Text animalNameTxt;
    [SerializeField] private Text foodTxt;
    [SerializeField] private Text soapTxt;
    [SerializeField] private Text toyTxt;

    [Header("Pause Menu Panel")]
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        levelManager = LevelManager.instance;
    }

    // ------------------ [BEGIN] Create Animal Panel -------------------
    public void OpenCreateAnimalPanel()
    {
        Time.timeScale = 0f;
        createAnimalPanel.SetActive(true);
    }

    public void OnCreateBtn()
    {
        Time.timeScale = 1f;
        createAnimalPanel.SetActive(false);
        Instantiate(listOfAnimalsPrefabs[animalSelected], new Vector3(0, 0, 0), Quaternion.identity);
        levelManager.CreateAnimal(listOfAnimalsPrefabs[animalSelected].GetComponent<AnimalController>(), nameInput, animalSelected);
    }

    public void OnAnimalSelectionDropdown(int input)
    {
        animalSelected = input;
        animalCreateImg = listOfAnimalsImages[input];

        switch (input)
        {
            case 0:
                animalDescriptionTxt.text = "This animal eats more than usual!";
                break;
            case 1:
                animalDescriptionTxt.text = "This animal gets more dirty than usual!";
                break;
            case 2:
                animalDescriptionTxt.text = "This animal likes to play more than usual!";
                break;
        }
    }

    public void OnAnimalNameInputInfield(string name) { nameInput = name; }

    // ------------ [END] Create Animal Panel ----------------------

    // ------------ [BEGIN] Animal Stats Panel --------------------

    public void OpenAnimalStatsPanel(GameObject gameObj)
    {
        if (!gameObj.GetComponent<AnimalController>()) { return; }

        else
        {
            Time.timeScale = 0f;
            animalStatsPanel.SetActive(true);

            animalSelectedController = gameObj.GetComponent<AnimalController>();
            animalStatsImg = listOfAnimalsImages[animalSelectedController.GetAnimalID()];
            animalNameTxt.text = animalSelectedController.GetAnimalName();
            foodTxt.text = levelManager.food.ToString();
            soapTxt.text = levelManager.soaps.ToString();
            toyTxt.text = levelManager.toys.ToString();
        }
    }

    public void OnAnimalFeed()
    {
        levelManager.UpdateFood(false);
        animalSelectedController.SetFedLevel(true);
    }

    public void OnAnimalClean()
    {
        levelManager.UpdateSoaps(false);
        animalSelectedController.SetCleanessLevel(true);
    }

    public void OnAnimalPlay()
    {
        levelManager.UpdateToys(false);
        animalSelectedController.SetPleasureLevel(true);
    }

    public void OnAnimalStatsResume()
    {
        Time.timeScale = 1f;
        animalStatsPanel.SetActive(false);
    }

    // ------------ [END] Animal Stats Panel --------------------

    // ------------ [BEGIN] Pause Menu Panel --------------------

    public void OpenPauseMenuPanel(bool isPaused)
    {
        if (isPaused)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            this.isPaused = isPaused;
        }
    }

    public void OnPauseMenuResume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public bool GameIsPaused() { return isPaused; }

    // ------------ [END] Pause Menu Panel ---------------------
}
