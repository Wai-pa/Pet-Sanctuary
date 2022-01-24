using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private UIManager uiManager;
    private LevelManager levelManager;

    [Header("Status")]
    [SerializeField] private int animalID;
    [SerializeField] private string animalName;
    [SerializeField] private int fedLevel;
    [SerializeField] private int cleanessLevel;
    [SerializeField] private int pleasureLevel;
    [SerializeField] private int overallLevel;
    [SerializeField] private float spawnX;
    [SerializeField] private float spawnY;

    [Header("Miscellaneous")]
    [SerializeField] private float timeToDecreaseLevel;
    [SerializeField] private float tempTime;

    void Start()
    {
        uiManager = UIManager.instance;
        levelManager = LevelManager.instance;

        fedLevel = 8;
        cleanessLevel = 8;
        pleasureLevel = 8;
        tempTime = 40f;
        timeToDecreaseLevel = 60f;
    }

    void Update()
    {
        levelManager.AnimalDatabaseUpdate(this);

        if (!uiManager.GameIsPaused())
        {
            if (uiManager.isAnimalStatsPanelOpen) 
            {
                uiManager.SetSatisfaction(GetOverallLevel());
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

    public void SetSpawnX(float spawnX) { this.spawnX = spawnX; }

    public float GetSpawnX() { return spawnX; }

    public float GetSpawnY() { return spawnY; }

    public void SetSpawnY(float spawnY) { this.spawnY = spawnY; }

    public int GetOverallLevel() 
    {
        overallLevel = (fedLevel + cleanessLevel + pleasureLevel) / 3;
        return overallLevel; 
    }
}
