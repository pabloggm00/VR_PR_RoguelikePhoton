using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private ElementType type;
    public SpriteRenderer spriteRenderer;



    public void Setup(Vector2 shootDirection, float bulletSpeed, int playerDamage, ElementType playerElement)
    {
        direction = shootDirection;
        speed = bulletSpeed;
        damage = playerDamage;
        type = playerElement;

        spriteRenderer.color = ElementsInteractions.GetElementColor(type);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Enemy>(out Enemy enemigo)) 
        {
            enemigo.TakeDamage(damage, type);
        }
        
        // Colisión con cualquier objeto, desactiva la bala
        DesactivateBullet();
    }

    

    private void DesactivateBullet()
    {
        gameObject.SetActive(false); // Desactivar la bala
    }
}
