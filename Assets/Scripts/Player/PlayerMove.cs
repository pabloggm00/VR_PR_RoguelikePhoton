using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad base del personaje
    public float smoothness = 0.1f; // Suavidad del deslizamiento

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 currentVelocity; // Para el deslizamiento
    private SpriteRenderer spriteRenderer;

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

        UpdateSpriteFlip();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    { 
        movementInput = movementInput.normalized;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void UpdateSpriteFlip()
    {
        // Flipeamos horizontalmente si disparamos hacia la izquierda o derecha
        if (movementInput == Vector2.right)
            spriteRenderer.flipX = false;
        else if (movementInput == Vector2.left)
            spriteRenderer.flipX = true;


        //tambien hay que hacerlo con el vertical
    }

    void Move()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movementInput * moveSpeed, ref currentVelocity, smoothness);
    }

}
