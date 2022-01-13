using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Instances")]
    private GameManager gameManager;
    private LevelManager levelManager;
    public static UIManager instance = null;

    [Header("UI")]
    [SerializeField] private Text foodText;
    [SerializeField] private Text soapText;
    [SerializeField] private Text toyText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text woodText;
    [SerializeField] private Text rateText;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
    }

    void Update()
    {
        FoodTextUpdate();
        SoapTextUpdate();
        ToyTextUpdate();
        MoneyTextUpdate();
        WoodTextUpdate();
        //RateTextUpdate();
    }

    void FoodTextUpdate() { foodText.text = levelManager.food.ToString(); }

    void SoapTextUpdate() { soapText.text = levelManager.soaps.ToString(); }

    void ToyTextUpdate() { toyText.text = levelManager.toys.ToString(); }

    void MoneyTextUpdate() { moneyText.text = levelManager.money.ToString(); }

    void WoodTextUpdate() { woodText.text = levelManager.wood.ToString(); }

    void RateTextUpdate()
    {

    }
}
