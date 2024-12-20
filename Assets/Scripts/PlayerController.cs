using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad base del personaje
    public float smoothness = 0.1f; // Suavidad del deslizamiento

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 currentVelocity; // Para el deslizamiento

    private void OnEnable()
    {
        // Conectar las acciones del InputManager
        InputManager.playerControls.Player.Move.performed += OnMoveInput;
        InputManager.playerControls.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        // Desconectar las acciones del InputManager
        InputManager.playerControls.Player.Move.performed -= OnMoveInput;
        InputManager.playerControls.Player.Move.canceled -= OnMoveInput;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        // Leer el movimiento del input
        movementInput = context.ReadValue<Vector2>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    { 
        movementInput = movementInput.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movementInput * moveSpeed, ref currentVelocity, smoothness);
    }
}
