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
    public PlayerSprites enemySpritesSettings;

    private float currentHP;
    private List<ElementSprite> elements;
    private ElementSprite elementCurrent;
    private DamageEffect damageEffect;

    // Evento para notificar al spawner
    public static event Action<GameObject> OnEnemyDeath;

    private void Start()
    {
        
        Init();
    }

    public void Init()
    {
        currentHP = hP;

        damageEffect = GetComponent<DamageEffect>();
        damageEffect.Init(spriteRenderer);

        elements = new List<ElementSprite>();
        enemySpritesSettings.AgregarSprites(elements);


        int index = UnityEngine.Random.Range(0, elements.Count);
        photonView.RPC("SyncElementSprite", RpcTarget.AllBuffered, index);
    }

    [PunRPC]
    public void SyncElementSprite(int index)
    {
        if (index >= 0 && index < elements.Count)
        {
            elementCurrent = elements[index];
            spriteRenderer.sprite = elementCurrent.sprite;
        }
        else
        {
            Debug.LogError("Índice de sprite inválido recibido.");
        }
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
            GetComponent<PhotonView>().RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Die()
    {
        OnEnemyDeath?.Invoke(this.gameObject); // Notificar al spawner
        Destroy(gameObject); // Eliminar al enemigo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, elementCurrent.elementType);
        }

       
    }


}
