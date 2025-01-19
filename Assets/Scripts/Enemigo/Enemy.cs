using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : MonoBehaviourPunCallbacks
{
    public float hP;
    public int damage;
    public SpriteRenderer spriteRenderer;
    public ElementSprite elementCurrent;

    public float currentHP;
    private DamageEffect damageEffect;

    // Evento para notificar al spawner
    public static event Action<int> OnEnemyDeath;

    private void Start()
    {
        
        Init();
    }

    public void Init()
    {
        currentHP = hP;

        damageEffect = GetComponent<DamageEffect>();
        damageEffect.Init(spriteRenderer);


    }


    [PunRPC]
    public void TakeDamage(int dmg, ElementType bulletElement)
    {
     

        float elementDamage = dmg * elementCurrent.GetMultiplierDamage(bulletElement);

        currentHP -= elementDamage;

        damageEffect.ApplyDamageBlink();

        if (currentHP <= 0)
        {
            currentHP = 0; // Muere
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<PhotonView>().RPC("Die", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void Die()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            OnEnemyDeath?.Invoke(photonView.ViewID); // Notificar al spawner
            PhotonNetwork.Destroy(gameObject); // Eliminar al enemigo
        }
        else
        {
            Destroy(gameObject); // Solo destruir localmente si no es MasterClient
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, elementCurrent.elementType);
        }

       
    }


}
