using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPunCallbacks
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

    PlayerController controller;

    private bool isInvulnerable = false; 

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    [PunRPC]
    public void TakeDamage(int damage, ElementType enemyElement)
    {
        if (!photonView.IsMine) return;
        if (isInvulnerable) return;

        float elementDamage = damage * controller.elementCurrent.GetMultiplierDamage(enemyElement);

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
