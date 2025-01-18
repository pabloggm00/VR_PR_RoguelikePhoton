using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviourPunCallbacks
{

    [Header("Configuración del player")]
    PlayerController controller;
    public SpriteRenderer spriteRenderer;
    private PlayerMove playerMove;

    [Header("Configuración de disparo")]
    public float bulletSpeed = 10f; //Velocidad de la bala     
    public int bulletDamage = 1; //Daño personaje     
    public float shootCooldown = 0.5f; //Daño personaje     
    [SerializeField] private GameObject bulletPrefab; //Para instanciar por si nos quedamos sin ninguna
    [SerializeField] private Transform spawnBullet; //Donde spawnea la bala
    private GameObject poolParent; //La piscina que contiene las balas
    private Vector2 shootDirection = Vector2.zero;
    bool canShoot = true;

    private List<GameObject> bulletPool = new(); //Lista de balas en la pool
    private List<GameObject> activeBullets = new(); //Balas activas
    private Camera mainCamera;


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

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        controller = GetComponent<PlayerController>();
        mainCamera = Camera.main;

        
    }

    [PunRPC]
    public void SetPoolParent(string nickname)
    {
        poolParent = new GameObject("Pool Parent " + nickname);
    }

    private void Update()
    {
        if (shootDirection != Vector2.zero && canShoot)
        {
            Shoot();
            //GetComponent<PhotonView>().RPC("Shoot", RpcTarget.All);
            canShoot = false;
        }
    }

    void UpdateSpriteFlip()
    {
        // Flipeamos horizontalmente si disparamos hacia la izquierda o derecha
        if (shootDirection == Vector2.right)
            spriteRenderer.flipX = false;
        else if (shootDirection == Vector2.left)
            spriteRenderer.flipX = true;
    }

    void OnShootInput(InputAction.CallbackContext context)
    {
        shootDirection = context.ReadValue<Vector2>();

        UpdateSpriteFlip();
    }

    void OnShootInputCancel(InputAction.CallbackContext context)
    {
        shootDirection = context.ReadValue<Vector2>();

        //playerMove.UpdateSpriteFlip(); //Recolocamos al personaje
    }

    [PunRPC]
    public void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, spawnBullet.position, Quaternion.identity);
        bullet.transform.SetParent(poolParent.transform);

        //Bullet bulletScript = bullet.GetComponent<Bullet>();
        bullet.GetComponent<PhotonView>().RPC("Setup", RpcTarget.All, shootDirection.normalized, bulletSpeed, bulletDamage, controller.elementCurrent.elementType);
        //bulletScript.Setup(shootDirection.normalized, bulletSpeed, bulletDamage, controller.elementCurrent.elementType);

        StartCoroutine(CooldownShoot());
    }
    /*void Shoot()
    {

        // Comprobamos qué balas de la lista de balas en uso
        // se han descativado al colisionar y las devolvemos a la pool.
        foreach (GameObject bullet in activeBullets)
        {
            if (bullet.activeInHierarchy) continue;

            bulletPool.Add(bullet);
            activeBullets.Remove(bullet);
            break;
        }

        // Creamos una variable para almacenar la bala elegida.
        GameObject chosenBullet;

        // Si hay balas en la pool...
        if (bulletPool.Count > 0)
        {
            // Sacamos la primera y la movemos de la pool a la lista de balas en uso.
            chosenBullet = bulletPool[0];
            bulletPool.Remove(chosenBullet);
            activeBullets.Add(chosenBullet);
        }
        // Si no hay ninguna disponible...
        else
        {
            // La instanciamos para nunca quedarnos sin balas.
            chosenBullet = PhotonNetwork.Instantiate(bulletPrefab.name, spawnBullet.position, Quaternion.identity);
            chosenBullet.transform.SetParent(poolParent.transform);
            activeBullets.Add(chosenBullet);
        }

        // Movemos la bala elegida a la posición de disparo y la activamos.
        chosenBullet.transform.position = spawnBullet.position;
        chosenBullet.SetActive(true);

        Bullet bulletScript = chosenBullet.GetComponent<Bullet>();
        bulletScript.Setup(shootDirection.normalized, bulletSpeed, bulletDamage, controller.elementCurrent.elementType);

        StartCoroutine(CooldownShoot());
    }*/

    IEnumerator CooldownShoot()
    {
        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }



}


