using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{

    [Header("Configuración del player")]
    public ElementType element;
    public SpriteRenderer spriteRenderer;

    [Header("Configuración de disparo")]
    public float bulletSpeed = 10f; //Velocidad de la bala     
    public int bulletDamage = 1; //Daño personaje     
    [SerializeField] private GameObject bulletPrefab; //Para instanciar por si nos quedamos sin ninguna
    [SerializeField] private Transform spawnBullet; //Donde spawnea la bala
    [SerializeField] private GameObject poolParent; //La piscina que contiene las balas

    private List<GameObject> bulletPool = new(); //Lista de balas en la pool
    private List<GameObject> activeBullets = new(); //Balas activas
    private Camera mainCamera;


    private void OnEnable()
    {
        InputManager.playerControls.Player.Shoot.performed += OnShootInput;
    }

    private void OnDisable()
    {
        InputManager.playerControls.Player.Shoot.performed -= OnShootInput;
    }

    void Start()
    {
        mainCamera = Camera.main;

        spriteRenderer.color = ElementsInteractions.GetElementColor(element);
    }

    void OnShootInput(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void Shoot()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //Cogemos la dirección 
        Vector2 shootDirection = (mousePosition - (Vector2)transform.position).normalized;

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
            chosenBullet = Instantiate(bulletPrefab, spawnBullet.position, Quaternion.identity, poolParent.transform);
            activeBullets.Add(chosenBullet);
        }

        // Movemos la bala elegida a la posición de disparo y la activamos.
        chosenBullet.transform.position = spawnBullet.position;
        chosenBullet.SetActive(true);

        Bullet bulletScript = chosenBullet.GetComponent<Bullet>();
        bulletScript.Setup(shootDirection, bulletSpeed, bulletDamage, element);
    }


}


