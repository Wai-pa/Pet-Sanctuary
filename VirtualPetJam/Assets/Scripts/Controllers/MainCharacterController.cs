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
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        soundManager = SoundManager.instance;
        rb = GetComponent<Rigidbody2D>();
        uiManager = UIManager.instance;
    }

    void FixedUpdate()
    {
        Movement();
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
            levelManager.RestoreSpawnedWoodTransform(collision.gameObject.transform.position);
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
                uiManager.OpenAnimalStatsPanel(collision.gameObject);
            }
        }
    }
}
