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
        if (photonView.IsMine)
        {
            // Conectar las acciones del InputManager
            InputManager.playerControls.Player.Move.performed += OnMoveInput;
            InputManager.playerControls.Player.Move.canceled += OnMoveInput;
        }
    }

    public override void OnDisable()
    {
        if (photonView.IsMine)
        {
            // Desconectar las acciones del InputManager
            InputManager.playerControls.Player.Move.performed -= OnMoveInput;
            InputManager.playerControls.Player.Move.canceled -= OnMoveInput;
        }
           
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        // Leer el movimiento del input
        movementInput = context.ReadValue<Vector2>();

        if (photonView.IsMine)
        {
            photonView.RPC("UpdateSpriteFlip", RpcTarget.AllBuffered, null);
        }
      
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
        if (photonView.IsMine) 
            Move();
    }

    [PunRPC]
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
