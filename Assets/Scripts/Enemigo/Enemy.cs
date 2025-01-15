using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hP;
    public int damage;
    public ElementSprite element;
    public SpriteRenderer spriteRenderer;

    public float currentHP;

    private DamageEffect damageEffect;

    // Evento para notificar al spawner
    public delegate void EnemyDeathHandler(Enemy enemy);
    public static event EnemyDeathHandler OnEnemyDeath;

    private void Start()
    {
        currentHP = hP;

        damageEffect = GetComponent<DamageEffect>();
        damageEffect.Init(spriteRenderer);
    }

    ElementSprite GetRandomElement()
    {
        

        return element;
    }

    public void TakeDamage(int dmg, ElementType bulletElement)
    {
        float elementDamage = dmg * element.GetMultiplierDamage(bulletElement);

        currentHP -= elementDamage;

        damageEffect.ApplyDamageBlink();

        if (currentHP <= 0)
        {
            currentHP = 0; // Muere
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke(this); // Notificar al spawner
        Destroy(gameObject); // Eliminar al enemigo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            Debug.Log("Hola");
            player.TakeDamage(damage, element.elementType);
        }

       
    }


}
