using System;
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
    DamageEffect damageEffect;


    PlayerController controller;

    private bool isInvulnerable = false;
    bool isDead;
    public static event Action<float, float> OnHealthChanged;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        damageEffect = GetComponent<DamageEffect>();
        damageEffect.Init(spriteRenderer);

        if (photonView.IsMine)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, ElementType enemyElement)
    {

        if (isDead) return;

        photonView.RPC("ApplyDamageBlink", RpcTarget.All);

        if (!photonView.IsMine) return; // Solo afecta al jugador local

        if (isInvulnerable) return;


        float elementDamage = damage * controller.elementCurrent.GetMultiplierDamage(enemyElement);

        currentHealth -= damage;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        StartCoroutine(InvulnerableTime());
    }

    IEnumerator InvulnerableTime()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(2);

        isInvulnerable = false;
    }

    [PunRPC]
    public void ApplyDamageBlink()
    {
        damageEffect.ApplyDamageBlink();
    }

    [PunRPC]
    public void StopApply()
    {
        damageEffect.StopApply();
    }

    private void Die()
    {
        spriteRenderer.sprite = controller.elementCurrent.spriteMuerto;
        GetComponent<PlayerMove>().Muerto();
        GetComponent<PlayerMove>().enabled = false;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

}
