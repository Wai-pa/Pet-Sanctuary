using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public static UIManager instance = null;
    private LevelManager levelManager;

    [Header("Ingame Gameplay UI Panel")]
    [SerializeField] private GameObject gameplayUIPanel;
    [SerializeField] private Text foodText;
    [SerializeField] private Text soapText;
    [SerializeField] private Text toyText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text woodText;
    [SerializeField] private Text rateText;

    [Header("Main Menu Panel")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;

    [Header("Create Animal Panel")]
    [SerializeField] private GameObject createAnimalPanel;
    [SerializeField] private Image animalCreateImg;
    [SerializeField] private Text animalDescriptionTxt;
    [SerializeField] private string nameInput;
    [SerializeField] private int animalSelected;
    [SerializeField] private Dropdown animalDropdown;
    [SerializeField] private InputField animalInputField;

    [Header("Animal Stats Panel")]
    [SerializeField] private GameObject animalStatsPanel;
    [SerializeField] private Image animalStatsImg;
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

    [Header("Fast Travel Black Panel")]
    [SerializeField] private GameObject fastTravelBlackPanel;

    [Header("Resources")]
    [SerializeField] private List<Sprite> listOfAnimalsImages = new List<Sprite>();
    [SerializeField] private AnimalController animalSelectedController = null;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        Time.timeScale = 0f;
        levelManager = LevelManager.instance;
        mainMenuPanel.SetActive(true);
    }

    void Update()
    {
        if (gameplayUIPanel.activeInHierarchy)
        {
            FoodTextUpdate();
            SoapTextUpdate();
            ToyTextUpdate();
            MoneyTextUpdate();
            WoodTextUpdate();
            RateTextUpdate();
        }
    }

    // ================= [BEGIN] Ingame Gameplay UI Panel ======================

    void FoodTextUpdate() { foodText.text = levelManager.food.ToString(); }

    void SoapTextUpdate() { soapText.text = levelManager.soaps.ToString(); }

    void ToyTextUpdate() { toyText.text = levelManager.toys.ToString(); }

    void MoneyTextUpdate() { moneyText.text = levelManager.money.ToString(); }

    void WoodTextUpdate() { woodText.text = levelManager.wood.ToString(); }

    void RateTextUpdate() { rateText.text = levelManager.rate.ToString(); }

    // ================= [END] Ingame Gameplay UI Panel =======================


    // ================= [BEGIN] Main Menu Panel ======================

    public void OnNewGame()
    {
        TogglePanels(gameplayUIPanel, mainMenuPanel);
        Time.timeScale = 1f;
    }

    public void OnLoadGame()
    {
        TogglePanels(gameplayUIPanel, mainMenuPanel);
        Time.timeScale = 1f;
    }

    public void OpenCreditsPanel() { TogglePanels(creditsPanel, mainMenuPanel); }

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

    public void OnCreditsBackBtn() { TogglePanels(mainMenuPanel, creditsPanel); }

    // ================= [END] Credits Panel ========================


    // ================= [BEGIN] Create Animal Panel =================
    public void OpenCreateAnimalPanel()
    {
        TogglePanels(createAnimalPanel, gameplayUIPanel);
        Time.timeScale = 0f;
    }

    public void OnCreateBtn()
    {
        if(nameInput == "") { return; }

        if(levelManager.GetSizeOfAnimalSpawnPointList() < 1) { return; }

        levelManager.CreateAnimal(nameInput, animalSelected);
        TogglePanels(gameplayUIPanel, createAnimalPanel);
        Time.timeScale = 1f;
    }

    public void OnAnimalSelectionDropdown()
    {
        animalSelected = animalDropdown.value;
        animalCreateImg.sprite = listOfAnimalsImages[animalDropdown.value];

        if (animalDropdown.value == 0) { animalDescriptionTxt.text = "This animal eats more than usual!"; }
        if (animalDropdown.value == 1) { animalDescriptionTxt.text = "This animal gets more dirty than usual!"; }
        if (animalDropdown.value == 2) { animalDescriptionTxt.text = "This animal likes to play more than usual!"; }
    }

    public void OnAnimalNameInputField() { nameInput = animalInputField.text; }

    public void OnCreateAnimalBack()
    {
        TogglePanels(gameplayUIPanel, createAnimalPanel);
        Time.timeScale = 1f;
    }

    // ================= [END] Create Animal Panel =====================


    // ================= [BEGIN] Animal Stats Panel ====================
    
    public void OpenAnimalStatsPanel(GameObject gameObj)
    {
        if (!gameObj.GetComponent<AnimalController>()) { return; }

        else
        {
            TogglePanels(animalStatsPanel, gameplayUIPanel);
            Time.timeScale = 0f;

            animalSelectedController = gameObj.GetComponent<AnimalController>();
            animalStatsImg.sprite = listOfAnimalsImages[animalSelectedController.GetAnimalID()];
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
        TogglePanels(gameplayUIPanel, animalStatsPanel);
        Time.timeScale = 1f;
    }

    // ============== [END] Animal Stats Panel ======================


    // ============== [BEGIN] Resource Trader Panel =================

    public void OpenResourceTraderPanel()
    {
        TogglePanels(resourceTraderPanel, gameplayUIPanel);
        woodResourceTraderText.text = levelManager.wood.ToString();
        moneyResourceTraderText.text = levelManager.money.ToString();
        foodResourceTraderText.text = levelManager.food.ToString();
        soapResourceTraderText.text = levelManager.soaps.ToString();
        toyResourceTraderText.text = levelManager.toys.ToString();
        Time.timeScale = 0f;
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
        TogglePanels(gameplayUIPanel, resourceTraderPanel);
        Time.timeScale = 1f;
    }

    // ============== [END] Resource Trader Panel ===================


    // ============== [BEGIN] Pause Menu Panel ======================

    public void OpenPauseMenuPanel(bool isPaused)
    {
        if (isPaused)
        {
            TogglePanels(pauseMenuPanel, gameplayUIPanel);
            Time.timeScale = 0f;
            this.isPaused = isPaused;
        }
    }

    public void OnPauseMenuResume()
    {
        TogglePanels(gameplayUIPanel, pauseMenuPanel);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnMainMenu()
    {
        TogglePanels(mainMenuPanel, pauseMenuPanel);
        Time.timeScale = 0f;
    }

    public void OnPauseMenuQuit() { OnQuit(); }

    public bool GameIsPaused() { return isPaused; }

    // ================= [END] Pause Menu Panel =======================


    // ================= [BEGIN] Fast Travel Black Panel =======================

    public void FastTravelBlackPanel(bool activate)
    {
        if (activate) { fastTravelBlackPanel.SetActive(true); }
        else { fastTravelBlackPanel.SetActive(false); }
    }

    // ================= [END] Fast Travel Black Panel =======================


    // ================= [BEGIN] Resources =======================

    void TogglePanels(GameObject panelToActivate, GameObject panelToDeactivate)
    {
        panelToActivate.SetActive(true);
        panelToDeactivate.SetActive(false);
    }

    // ================= [END] Resources =======================
}
