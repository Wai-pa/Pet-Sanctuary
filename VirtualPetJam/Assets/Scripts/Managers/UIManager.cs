using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    private LevelManager levelManager;
    private SoundManager soundManager;

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

    [Header("Instructions Panel")]
    [SerializeField] private GameObject instructionsPanel;

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
    [SerializeField] private GameObject messageBox;
    [SerializeField] private Text messageBoxTxt;

    [Header("Animal Stats Panel")]
    [SerializeField] private GameObject animalStatsPanel;
    [SerializeField] private Image animalStatsImg;
    [SerializeField] private Text animalNameTxt;
    [SerializeField] private Text foodAnimalStatsTxt;
    [SerializeField] private Text soapAnimalStatsTxt;
    [SerializeField] private Text toyAnimalStatsTxt;
    [SerializeField] private Slider satisfactionSlider;
    [SerializeField] private Gradient satisfactionGradient;
    [SerializeField] private Image sliderFillImage;
    public bool isAnimalStatsPanelOpen;
    [SerializeField] private Text fedAnimalStatsTxt;
    [SerializeField] private Text cleanessAnimalStatsTxt;
    [SerializeField] private Text pleasureAnimalStatsTxt;

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
    [SerializeField] private Slider volumeSlider;

    [Header("Fast Travel Black Panel")]
    [SerializeField] private GameObject fastTravelBlackPanel;

    [Header("Resources")]
    [SerializeField] private List<Sprite> listOfAnimalsImages = new List<Sprite>();
    [SerializeField] private AnimalController animalSelectedController = null;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip animalEatClip;
    [SerializeField] private AudioClip animalCleanClip;
    [SerializeField] private AudioClip animalPlayClip;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        levelManager = LevelManager.instance;
        soundManager = SoundManager.instance;
        Time.timeScale = 0f;
        volumeSlider.value = 0.5f;
        soundManager.ChangeMasterVolume(volumeSlider.value);
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

    void RateTextUpdate() { rateText.text = levelManager.rate.ToString() + "/10"; }

    // ================= [END] Ingame Gameplay UI Panel =======================


    // ================= [BEGIN] Main Menu Panel ======================

    public void OnNewGame() { TogglePanels(instructionsPanel, mainMenuPanel); }

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


    // ================= [BEGIN] Instructions Panel ======================

    public void OnContinue() 
    {
        isPaused = false;
        TogglePanels(gameplayUIPanel, instructionsPanel);
        Time.timeScale = 1f;
    }

    public void OnInstructionsPanelBack() { TogglePanels(mainMenuPanel, instructionsPanel); }

    // ================= [END] Instructions Panel =======================


    // ================= [BEGIN] Credits Panel ======================

    public void OnCreditsBackBtn() { TogglePanels(mainMenuPanel, creditsPanel); }

    // ================= [END] Credits Panel ========================


    // ================= [BEGIN] Create Animal Panel =================
    public void OpenCreateAnimalPanel()
    {
        TogglePanels(createAnimalPanel, gameplayUIPanel);
        animalInputField.text = "";
        animalDropdown.value = 0;
        Time.timeScale = 0f;
        if (levelManager.GetNumberOfAnimals() > 5)
        {
            messageBox.SetActive(true);
            messageBoxTxt.text = "There is no more room for animals!";
        }
        else { messageBox.SetActive(false); }
    }

    public void OnCreateBtn()
    {
        if (levelManager.money < 10)
        {
            messageBox.SetActive(true);
            messageBoxTxt.text = "It costs 10 coins to create an animal!";
            return;
        }

        else if (nameInput == "")
        {
            messageBox.SetActive(true);
            messageBoxTxt.text = "Hey, you're forgetting the name...";
            return; 
        }

        else if (levelManager.AnimalNameAlreadyExists(nameInput)) 
        {
            messageBox.SetActive(true);
            messageBoxTxt.text = "This name already exists!";
            return; 
        }

        else if (levelManager.GetNumberOfAnimals() > 5) 
        {
            messageBox.SetActive(true);
            messageBoxTxt.text = "There is no more room for animals!";
            return; 
        }

        else
        {
            levelManager.CreateAnimal(nameInput, animalSelected);
            levelManager.money -= 10;
            TogglePanels(gameplayUIPanel, createAnimalPanel);
            Time.timeScale = 1f;
        }
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
            isAnimalStatsPanelOpen = true;
            TogglePanels(animalStatsPanel, gameplayUIPanel);
            Time.timeScale = 0f;

            animalSelectedController = gameObj.GetComponent<AnimalController>();
            animalStatsImg.sprite = listOfAnimalsImages[animalSelectedController.GetAnimalID() - 1];
            animalNameTxt.text = animalSelectedController.GetAnimalName();
            foodAnimalStatsTxt.text = levelManager.food.ToString();
            soapAnimalStatsTxt.text = levelManager.soaps.ToString();
            toyAnimalStatsTxt.text = levelManager.toys.ToString();
            SetMaxSatisfaction(10);
            SetSatisfaction(animalSelectedController.GetOverallLevel());
        }
    }

    public void OnAnimalFeed()
    {
        if(levelManager.food < 1 || animalSelectedController.GetFedLevel() > 9) { return; }

        soundManager.PlaySound(animalEatClip);
        levelManager.UpdateFood(false);
        animalSelectedController.SetFedLevel(true);
        foodAnimalStatsTxt.text = levelManager.food.ToString();
        SetSatisfaction(animalSelectedController.GetOverallLevel());
    }

    public void OnAnimalClean()
    {
        if (levelManager.soaps < 1 || animalSelectedController.GetCleanessLevel() > 9) { return; }

        soundManager.PlaySound(animalCleanClip);
        levelManager.UpdateSoaps(false);
        animalSelectedController.SetCleanessLevel(true);
        soapAnimalStatsTxt.text = levelManager.soaps.ToString();
        SetSatisfaction(animalSelectedController.GetOverallLevel());
    }

    public void OnAnimalPlay()
    {
        if (levelManager.toys < 1 || animalSelectedController.GetPleasureLevel() > 9) { return; }

        soundManager.PlaySound(animalPlayClip);
        levelManager.UpdateToys(false);
        animalSelectedController.SetPleasureLevel(true);
        toyAnimalStatsTxt.text = levelManager.toys.ToString();
        SetSatisfaction(animalSelectedController.GetOverallLevel());
    }

    public void SetMaxSatisfaction(int satisfaction)
    {
        satisfactionSlider.maxValue = satisfaction;
        sliderFillImage.color = satisfactionGradient.Evaluate(1f);
    }

    public void SetSatisfaction(int satisfaction)
    {
        satisfactionSlider.value = satisfaction;
        sliderFillImage.color = satisfactionGradient.Evaluate(satisfactionSlider.normalizedValue);
    }

    public void SetAnimalStatus()
    {
        fedAnimalStatsTxt.text = animalSelectedController.GetFedLevel().ToString() + "/10";
        cleanessAnimalStatsTxt.text = animalSelectedController.GetCleanessLevel().ToString() + "/10";
        pleasureAnimalStatsTxt.text = animalSelectedController.GetPleasureLevel().ToString() + "/10";
    }

    public void OnAnimalStatsResume()
    {
        isAnimalStatsPanelOpen = false;
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

    public void OnVolumeChanged()
    {
        soundManager.ChangeMasterVolume(volumeSlider.value);
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


    // ===================== [BEGIN] Resources ===========================

    void TogglePanels(GameObject panelToActivate, GameObject panelToDeactivate)
    {
        panelToActivate.SetActive(true);
        panelToDeactivate.SetActive(false);
    }

    // ===================== [END] Resources ============================
}
