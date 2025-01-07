using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Configuración de Vida")]
    public float maxHealth = 10f;
    public float currentHealth; 

    [Header("Efectos Visuales")]
    public SpriteRenderer spriteRenderer; 
    public Color damageColor = Color.red;
    public float damageBlinkDuration = 0.1f; // Duración del parpadeo
    public float damageBlinkDurationAndInvulnerableOffset = 0.1f; // Duración del parpadeo
    public int damageBlinkCount = 3; // Cantidad de parpadeos

    [HideInInspector]
    public ElementType element;

    private bool isInvulnerable = false; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, ElementType enemyElement)
    {
        if (isInvulnerable) return;

        float elementDamage = damage * ElementsInteractions.GetDamageMultiplier(enemyElement, element);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            StartCoroutine(ApplyDamageEffect());
        }
    }

    private IEnumerator ApplyDamageEffect()
    {
        isInvulnerable = true;
        GetComponent<CapsuleCollider2D>().enabled = false;

        Color originalColor = spriteRenderer.color;
        for (int i = 0; i < damageBlinkCount; i++)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(damageBlinkDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(damageBlinkDuration);
        }

        yield return new WaitForSeconds(damageBlinkDurationAndInvulnerableOffset);

        GetComponent<CapsuleCollider2D>().enabled = true;

        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
