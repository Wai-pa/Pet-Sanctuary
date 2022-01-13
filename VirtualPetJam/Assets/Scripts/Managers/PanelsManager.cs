using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{
    public static PanelsManager instance = null;
    private LevelManager levelManager;

    [Header("Resources")]
    [SerializeField] private GameObject gameplayUIPanel;
    [SerializeField] private List<GameObject> listOfAnimalsPrefabs = new List<GameObject>();
    [SerializeField] private List<Sprite> listOfAnimalsImages = new List<Sprite>();
    [SerializeField] private AnimalController animalSelectedController = null;

    [Header("Main Menu Panel")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;

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
    [SerializeField] private Text foodAnimalStatsTxt;
    [SerializeField] private Text soapAnimalStatsTxt;
    [SerializeField] private Text toyAnimalStatsTxt;

    [Header("Resource Trader Panel")]
    [SerializeField] private GameObject resourceTraderPanel;
    [SerializeField] private Text foodResourceTraderText;
    [SerializeField] private Text soapResourceTraderText;
    [SerializeField] private Text toyResourceTraderText;
    [SerializeField] private Text moneyResourceTraderText;
    [SerializeField] private Text woodResourceTraderText;

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

    // ================= [BEGIN] Main Menu Panel ======================

    public void OnNewGame()
    {

    }

    public void OnLoadGame()
    {

    }

    public void OpenCreditsPanel()
    { 
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // ================= [END] Main Menu Panel =======================


    // ================= [BEGIN] Credits Panel ======================

    public void OnCreditsBackBtn() 
    { 
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ================= [END] Credits Panel ========================


    // ================= [BEGIN] Create Animal Panel =================
    public void OpenCreateAnimalPanel()
    {
        gameplayUIPanel.SetActive(false);
        Time.timeScale = 0f;
        createAnimalPanel.SetActive(true);
    }

    public void OnCreateBtn()
    {
        gameplayUIPanel.SetActive(true);
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

    // ================= [END] Create Animal Panel =====================


    // ================= [BEGIN] Animal Stats Panel ====================
    
    public void OpenAnimalStatsPanel(GameObject gameObj)
    {
        if (!gameObj.GetComponent<AnimalController>()) { return; }

        else
        {
            gameplayUIPanel.SetActive(false);
            Time.timeScale = 0f;
            animalStatsPanel.SetActive(true);

            animalSelectedController = gameObj.GetComponent<AnimalController>();
            animalStatsImg = listOfAnimalsImages[animalSelectedController.GetAnimalID()];
            animalNameTxt.text = animalSelectedController.GetAnimalName();
            foodAnimalStatsTxt.text = levelManager.food.ToString();
            soapAnimalStatsTxt.text = levelManager.soaps.ToString();
            toyAnimalStatsTxt.text = levelManager.toys.ToString();
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
        gameplayUIPanel.SetActive(true);
        Time.timeScale = 1f;
        animalStatsPanel.SetActive(false);
    }

    // ============== [END] Animal Stats Panel ======================


    // ============== [BEGIN] Resource Trader Panel =================

    public void OpenResourceTraderPanel()
    {
        gameplayUIPanel.SetActive(false);
        Time.timeScale = 0f;
        resourceTraderPanel.SetActive(true);
    }

    public void OnSellWood()
    {
        if(levelManager.wood > 0)
        {
            levelManager.UpdateWood(false);
            woodResourceTraderText.text = levelManager.wood.ToString();
            levelManager.UpdateMoney(true);
            moneyResourceTraderText.text = levelManager.money.ToString();
        }
    }

    public void OnBuyFood()
    {
        if(levelManager.money > 0)
        {
            levelManager.UpdateMoney(false);
            moneyResourceTraderText.text = levelManager.money.ToString();
            levelManager.UpdateFood(true);
            foodResourceTraderText.text = levelManager.food.ToString();
        }
    }

    public void OnBuySoaps()
    {
        if (levelManager.money > 0)
        {
            levelManager.UpdateMoney(false);
            moneyResourceTraderText.text = levelManager.money.ToString();
            levelManager.UpdateSoaps(true);
            soapResourceTraderText.text = levelManager.soaps.ToString();
        }
    }

    public void OnBuyToys()
    {
        if (levelManager.money > 0)
        {
            levelManager.UpdateMoney(false);
            moneyResourceTraderText.text = levelManager.money.ToString();
            levelManager.UpdateToys(true);
            toyResourceTraderText.text = levelManager.toys.ToString();
        }
    }

    public void OnResourceTraderFinish()
    {
        gameplayUIPanel.SetActive(true);
        Time.timeScale = 1f;
        resourceTraderPanel.SetActive(false);
    }

    // ============== [END] Resource Trader Panel ===================


    // ============== [BEGIN] Pause Menu Panel ======================

    public void OpenPauseMenuPanel(bool isPaused)
    {
        if (isPaused)
        {
            gameplayUIPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            this.isPaused = isPaused;
        }
    }

    public void OnPauseMenuResume()
    {
        gameplayUIPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnMainMenu()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnPauseMenuQuit() { OnQuit(); }

    public bool GameIsPaused() { return isPaused; }

    // ================= [END] Pause Menu Panel =======================
}
