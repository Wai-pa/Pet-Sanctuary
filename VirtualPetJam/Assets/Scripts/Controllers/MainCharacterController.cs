using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private float movementSpeed;
    private Rigidbody2D rb;
    private Vector2 inputVector;
    private Vector2 moveVector;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private bool isInteracted;

    [Header("Instances")]
    private LevelManager levelManager;
    private SoundManager soundManager;
    private UIManager uiManager;
    private Animator animator;

    [Header("Fast Travel")]
    private Vector3 spawnOutsideFrontDoor;
    private Vector3 spawnOutsideBackDoor;
    private Vector3 spawnBuildingFrontDoor;
    private Vector3 spawnBuildingBackDoor;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip openPanelClip;
    [SerializeField] private AudioClip openDoorClip;
    [SerializeField] private AudioClip collectWoodClip;
    [SerializeField] private AudioClip grassFootstepClip;
    [SerializeField] private AudioClip indoorFootstepClip;
    private AudioSource audioSource;

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
        levelManager = LevelManager.instance;
        soundManager = SoundManager.instance;
        rb = GetComponent<Rigidbody2D>();
        uiManager = UIManager.instance;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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

        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wood")
        {
            soundManager.PlaySound(collectWoodClip);
            levelManager.RestoreSpawnedWoodPosition(collision.gameObject.transform.position);
            levelManager.UpdateWood(true);
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isInteracted)
        {
            if (collision.gameObject.tag == "Animal") 
            {
                soundManager.PlaySound(openPanelClip);
                uiManager.OpenAnimalStatsPanel(collision.gameObject); 
            }
            else if(collision.gameObject.name == "OutsideFrontDoor") 
            {
                levelManager.IsPlayerInTheFrontyard(false);
                StartCoroutine(FastTravel(spawnBuildingFrontDoor)); 
            }
            else if (collision.gameObject.name == "OutsideBackDoor") 
            {
                levelManager.IsPlayerInTheBackyard(false);
                StartCoroutine(FastTravel(spawnBuildingBackDoor)); 
            }
            else if (collision.gameObject.name == "BuildingFrontDoor") 
            {
                levelManager.IsPlayerInTheFrontyard(true);
                StartCoroutine(FastTravel(spawnOutsideFrontDoor)); 
            }
            else if (collision.gameObject.name == "BuildingBackDoor") 
            {
                levelManager.IsPlayerInTheBackyard(true);
                StartCoroutine(FastTravel(spawnOutsideBackDoor)); 
            }
            else if (collision.gameObject.name == "AnimalCreator") 
            {
                soundManager.PlaySound(openPanelClip);
                uiManager.OpenCreateAnimalPanel(); 
            }

            else if (collision.gameObject.name == "ResourceTrader")
            {
                soundManager.PlaySound(openPanelClip);
                uiManager.OpenResourceTraderPanel(); 
            }
        }
    }

    IEnumerator FastTravel(Vector3 target)
    {
        Time.timeScale = 0f;
        soundManager.PlaySound(openDoorClip);
        uiManager.FastTravelBlackPanel(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        transform.position = target;
        uiManager.FastTravelBlackPanel(false);
    }

    public void PlayFootstep()
    {
        if(!levelManager.isPlayerInTheBackyard && !levelManager.isPlayerInTheFrontyard) { audioSource.PlayOneShot(indoorFootstepClip); }
        else { audioSource.PlayOneShot(grassFootstepClip); }
    }
}
