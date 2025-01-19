using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f; // Velocidad base del personaje
    public float smoothness = 0.1f; // Suavidad del deslizamiento

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 currentVelocity; // Para el deslizamiento
    private SpriteRenderer spriteRenderer;

    public override void OnEnable()
    {
 
        InputManager.playerControls.Player.Move.performed += OnMoveInput;
        InputManager.playerControls.Player.Move.canceled += OnMoveInput;
        
    }

    public override void OnDisable()
    {

       
        InputManager.playerControls.Player.Move.performed -= OnMoveInput;
        InputManager.playerControls.Player.Move.canceled -= OnMoveInput;
        
           
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
      
        movementInput = context.ReadValue<Vector2>();

        if (movementInput.x != 0) 
        {
            bool flipRight = movementInput.x > 0;
            UpdateSpriteFlip(flipRight);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!photonView.IsMine)
        {
            Destroy(rb);
        }

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

 
    public void UpdateSpriteFlip(bool flipRight)
    {
        spriteRenderer.flipX = !flipRight;
    }

    void Move()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movementInput * moveSpeed, ref currentVelocity, smoothness);
    }

}
