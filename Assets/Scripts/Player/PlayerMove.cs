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
 
        // Conectar las acciones del InputManager
        InputManager.playerControls.Player.Move.performed += OnMoveInput;
        InputManager.playerControls.Player.Move.canceled += OnMoveInput;
        
    }

    public override void OnDisable()
    {

        // Desconectar las acciones del InputManager
        InputManager.playerControls.Player.Move.performed -= OnMoveInput;
        InputManager.playerControls.Player.Move.canceled -= OnMoveInput;
        
           
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        // Leer el movimiento del input
        movementInput = context.ReadValue<Vector2>();

        if (movementInput.x != 0) // Solo actualizar cuando haya movimiento horizontal
        {
            // Determinar el flip basado en el input horizontal
            bool flipRight = movementInput.x > 0;
            photonView.RPC("UpdateSpriteFlip", RpcTarget.AllBuffered, flipRight);
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

    [PunRPC]
    public void UpdateSpriteFlip(bool flipRight)
    {
        spriteRenderer.flipX = !flipRight; // Flip al cambiar la dirección
    }

    void Move()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movementInput * moveSpeed, ref currentVelocity, smoothness);
    }

}
