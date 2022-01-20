using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPatrollingBehaviour : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float waitTime = 2f;
    private float tempTime;
    [SerializeField] private List<Transform> listOfAnimalPatrolPoints = new List<Transform>();
    [SerializeField] private int rnd = 1;

    [Header("Route 0")]
    [SerializeField] private Transform point01;
    [SerializeField] private Transform point02;
    [SerializeField] private Transform point03;

    [Header("Route 1")]
    [SerializeField] private Transform point11;
    [SerializeField] private Transform point12;
    [SerializeField] private Transform point13;
    [SerializeField] private Transform point14;

    [Header("Route 2")]
    [SerializeField] private Transform point21;
    [SerializeField] private Transform point22;

    [Header("Route 3")]
    [SerializeField] private Transform point31;
    [SerializeField] private Transform point32;
    [SerializeField] private Transform point33;

    [Header("Route 4")]
    [SerializeField] private Transform point41;
    [SerializeField] private Transform point42;
    [SerializeField] private Transform point43;
    [SerializeField] private Transform point44;

    [Header("Route 5")]
    [SerializeField] private Transform point51;
    [SerializeField] private Transform point52;
    [SerializeField] private Transform point53;

    void Start()
    {
        levelManager = LevelManager.instance;
        tempTime = waitTime;

        switch (levelManager.GetAnimalSpawnPointInt())
        {
            case 0:
                listOfAnimalPatrolPoints.Add(point01);
                listOfAnimalPatrolPoints.Add(point02);
                listOfAnimalPatrolPoints.Add(point03);
                break;
            case 1:
                listOfAnimalPatrolPoints.Add(point11);
                listOfAnimalPatrolPoints.Add(point12);
                listOfAnimalPatrolPoints.Add(point13);
                listOfAnimalPatrolPoints.Add(point14);
                break;
            case 2:
                listOfAnimalPatrolPoints.Add(point21);
                listOfAnimalPatrolPoints.Add(point22);
                break;
            case 3:
                listOfAnimalPatrolPoints.Add(point31);
                listOfAnimalPatrolPoints.Add(point32);
                listOfAnimalPatrolPoints.Add(point33);
                break;
            case 4:
                listOfAnimalPatrolPoints.Add(point41);
                listOfAnimalPatrolPoints.Add(point42);
                listOfAnimalPatrolPoints.Add(point43);
                listOfAnimalPatrolPoints.Add(point44);
                break;
            case 5:
                listOfAnimalPatrolPoints.Add(point51);
                listOfAnimalPatrolPoints.Add(point52);
                listOfAnimalPatrolPoints.Add(point53);
                break;
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, listOfAnimalPatrolPoints[rnd].position, movementSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, listOfAnimalPatrolPoints[rnd].position) <= 0.015f)
        {
            if (tempTime <= 0)
            {
                rnd = Random.Range(0, listOfAnimalPatrolPoints.Count);
                tempTime = waitTime;
            }
            else { tempTime -= Time.deltaTime; }
        }
    }
}
