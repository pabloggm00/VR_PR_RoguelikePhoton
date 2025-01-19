using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviourPunCallbacks
{

    [Header("Configuración del jugador")]
    private PlayerController controller;

    [Header("Configuración de disparo")]
    public float bulletSpeed = 10f; // Velocidad de la bala
    public int bulletDamage = 1; // Daño de la bala
    public float shootCooldown = 0.5f; // Tiempo entre disparos
    [SerializeField] private GameObject bulletPrefab; // Prefab de la bala
    [SerializeField] private Transform spawnBullet; // Punto de spawn de la bala

    private Vector2 shootDirection = Vector2.zero;
    private bool canShoot = true;

    public override void OnEnable()
    {
        InputManager.playerControls.Player.Shoot.performed += OnShootInput;
        InputManager.playerControls.Player.Shoot.canceled += OnShootInputCancel;
    }

    public override void OnDisable()
    {
        InputManager.playerControls.Player.Shoot.performed -= OnShootInput;
        InputManager.playerControls.Player.Shoot.canceled -= OnShootInputCancel;
    }

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (shootDirection != Vector2.zero && canShoot && photonView.IsMine)
        {
            Shoot();
            canShoot = false;
            StartCoroutine(CooldownShoot());
        }
    }

    private void OnShootInput(InputAction.CallbackContext context)
    {
        shootDirection = context.ReadValue<Vector2>();
    }

    private void OnShootInputCancel(InputAction.CallbackContext context)
    {
        shootDirection = Vector2.zero; // Resetea la dirección al soltar el input
    }

    private void Shoot()
    {
        // Instanciar la bala en la red
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, spawnBullet.position, Quaternion.identity);

        // Configurar la bala en todos los clientes
        bullet.GetComponent<PhotonView>().RPC(
            "Setup",
            RpcTarget.AllBuffered,
            shootDirection.normalized,
            bulletSpeed,
            bulletDamage,
            GetComponent<PlayerController>().elementCurrent.elementType
        );
    }

    private IEnumerator CooldownShoot()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

}


