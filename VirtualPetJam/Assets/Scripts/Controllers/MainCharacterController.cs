using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private float movementSpeed = 8f;
    private Vector2 inputVector;
    private Vector2 moveVector;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private bool isInteracted = false;

    [Header("Instances")]
    private GameManager gameManager;
    private LevelManager levelManager;
    private SoundManager soundManager;
    private CharacterController controller;
    private PanelsManager panelsManager;

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
        panelsManager.OpenPauseMenuPanel(isPaused);
    }

    void Start()
    {
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        soundManager = SoundManager.instance;
        controller = GetComponent<CharacterController>();
        panelsManager = PanelsManager.instance;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        moveVector.x = inputVector.x;
        moveVector.y = inputVector.y;
        controller.Move(moveVector * movementSpeed * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isInteracted)
        {
            if (hit.gameObject.CompareTag("Wood"))
            {
                levelManager.UpdateWood(true);
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.CompareTag("Animal"))
            {
                panelsManager.OpenAnimalStatsPanel(hit.gameObject);
            }
        }
    }
}
