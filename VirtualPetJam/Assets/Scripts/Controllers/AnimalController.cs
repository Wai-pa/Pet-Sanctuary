using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private UIManager uiManager;

    [Header("Status")]
    [SerializeField] private int animalID;
    [SerializeField] private string animalName;
    [SerializeField] private int fedLevel;
    [SerializeField] private int cleanessLevel;
    [SerializeField] private int pleasureLevel;
    [SerializeField] private int overallLevel;
    [SerializeField] private float timeToDecreaseLevel;
    [SerializeField] private float tempTime = 20f;

    void Start()
    {
        uiManager = UIManager.instance;

        fedLevel = 8;
        cleanessLevel = 8;
        pleasureLevel = 8;
        timeToDecreaseLevel = 40f;
    }

    void Update()
    {
        if (!uiManager.GameIsPaused())
        {
            overallLevel = GetOverallLevel();

            if (uiManager.isAnimalStatsPanelOpen) 
            {
                uiManager.SetSatisfaction(overallLevel);
                uiManager.SetAnimalStatus();
            }

            if (animalID == 1)
            {
                if (tempTime <= 0)
                {
                    if (fedLevel > 2) { fedLevel -= 2; }
                    if (cleanessLevel > 1) { cleanessLevel -= 1; }
                    if (pleasureLevel > 1) { pleasureLevel -= 1; }

                    tempTime = timeToDecreaseLevel;
                }
                else
                {
                    tempTime -= Time.unscaledDeltaTime;
                }
            }
            else if (animalID == 2)
            {
                if (tempTime <= 0)
                {
                    if (fedLevel > 1) { fedLevel -= 1; }
                    if (cleanessLevel > 2) { cleanessLevel -= 2; }
                    if (pleasureLevel > 1) { pleasureLevel -= 1; }

                    tempTime = timeToDecreaseLevel;
                }
                else
                {
                    tempTime -= Time.unscaledDeltaTime;
                }
            }
            else if (animalID == 3)
            {
                if (tempTime <= 0)
                {
                    if (fedLevel > 1) { fedLevel -= 1; }
                    if (cleanessLevel > 1) { cleanessLevel -= 1; }
                    if (pleasureLevel > 2) { pleasureLevel -= 2; }

                    tempTime = timeToDecreaseLevel;
                }
                else
                {
                    tempTime -= Time.unscaledDeltaTime;
                }
            }
        }
    }

    public string GetAnimalName() { return animalName; }

    public void SetAnimalName(string name) { animalName = name; }

    public int GetFedLevel() { return fedLevel; }

    public void SetFedLevel(bool increase) { fedLevel = increase ? fedLevel += 1 : fedLevel -= 1; }

    public int GetCleanessLevel() { return cleanessLevel; }

    public void SetCleanessLevel(bool increase) { cleanessLevel = increase ? cleanessLevel += 1 : cleanessLevel -= 1; }

    public int GetPleasureLevel() { return pleasureLevel; }

    public void SetPleasureLevel(bool increase) { pleasureLevel = increase ? pleasureLevel += 1 : pleasureLevel -= 1; }

    public int GetAnimalID() { return animalID; }

    public void SetAnimalID(int animalID) { this.animalID = animalID; }

    public int GetOverallLevel() 
    {
        overallLevel = (fedLevel + cleanessLevel + pleasureLevel) / 3;
        return overallLevel; 
    }
}
