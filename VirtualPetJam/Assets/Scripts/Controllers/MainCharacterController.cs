using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementSpeed = 8f;
    private Vector2 inputVector;
    private Vector2 moveVector;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private bool isInteracted;

    [Header("Instances")]
    private GameManager gameManager;
    private LevelManager levelManager;
    private SoundManager soundManager;
    private UIManager uiManager;

    [Header("Fast Travel")]
    private Vector3 spawnOutsideFrontDoor;
    private Vector3 spawnOutsideBackDoor;
    private Vector3 spawnBuildingFrontDoor;
    private Vector3 spawnBuildingBackDoor;

    public void OnMove(InputAction.CallbackContext context) // AD (Keyboard), Left Stick (Gamepad)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnInteraction(InputAction.CallbackContext context) // E (Keyboard), Button West (Gamepad)
    {
        isInteracted = context.performed;
    }

    public void OnPause(InputAction.CallbackContext context) // ESC (Keyboard), Button Start (Gamepad)
    {
        isPaused = context.performed;
        uiManager.OpenPauseMenuPanel(isPaused);
    }

    void Start()
    {
        Initialize();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Initialize()
    {
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        soundManager = SoundManager.instance;
        rb = GetComponent<Rigidbody2D>();
        uiManager = UIManager.instance;

        spawnOutsideFrontDoor = new Vector3(4, -1, 0);
        spawnOutsideBackDoor = new Vector3(4, 5, 0);
        spawnBuildingFrontDoor = new Vector3(25.5f, 1, 0);
        spawnBuildingBackDoor = new Vector3(25.5f, 4.4f, 0);
    }

    void Movement()
    {
        moveVector.x = inputVector.x;
        moveVector.y = inputVector.y;
        rb.MovePosition(rb.position + moveVector * movementSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wood")
        {
            levelManager.RestoreSpawnedWoodPosition(collision.gameObject.transform.position);
            levelManager.UpdateWood(true);
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isInteracted)
        {
            if (collision.gameObject.tag == "Animal") { uiManager.OpenAnimalStatsPanel(collision.gameObject); }
            else if(collision.gameObject.name == "OutsideFrontDoor") { StartCoroutine(FastTravel(spawnBuildingFrontDoor)); }
            else if (collision.gameObject.name == "OutsideBackDoor") { StartCoroutine(FastTravel(spawnBuildingBackDoor)); }
            else if (collision.gameObject.name == "BuildingFrontDoor") { StartCoroutine(FastTravel(spawnOutsideFrontDoor)); }
            else if (collision.gameObject.name == "BuildingBackDoor") { StartCoroutine(FastTravel(spawnOutsideBackDoor)); }
        }
    }

    IEnumerator FastTravel(Vector3 target)
    {
        if(target == spawnOutsideFrontDoor) { levelManager.IsPlayerInTheFrontyard(false); }
        else if(target == spawnOutsideBackDoor) { levelManager.IsPlayerInTheBackyard(false); }
        else if (target == spawnBuildingFrontDoor) { levelManager.IsPlayerInTheFrontyard(true); }
        else if (target == spawnBuildingBackDoor) { levelManager.IsPlayerInTheBackyard(true); }

        uiManager.FastTravelBlackPanel(true);
        yield return new WaitForSeconds(2f);
        transform.position = target;
        uiManager.FastTravelBlackPanel(false);
    }
}
