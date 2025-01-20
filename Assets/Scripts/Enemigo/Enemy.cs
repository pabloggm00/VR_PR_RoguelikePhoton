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

    [Header("Soul Settings")]
    public GameObject soulPrefab;
    public int minSouls = 1;
    public int maxSouls = 3;

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

        float multiplier = elementCurrent.GetMultiplierDamage(bulletElement);
        float elementDamage = dmg * multiplier;

        // Mostrar el daño recibido
        //photonView.RPC("ShowDamage", RpcTarget.All, elementDamage, multiplier);
        GetComponent<DamageDisplay>().ShowDamage(elementDamage, multiplier);

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

        SpawnSouls();

        if (PhotonNetwork.IsMasterClient)
        {
            OnEnemyDeath?.Invoke(photonView.ViewID); // Notificar al spawner
            //photonView.RPC("SpawnSouls", RpcTarget.AllBuffered);
            PhotonNetwork.Destroy(gameObject); // Eliminar al enemigo
        }
        else
        {
            Destroy(gameObject); // Solo destruir localmente si no es MasterClient
        }


    }

    [PunRPC]
    public void SpawnSouls()
    {
        int soulCount = UnityEngine.Random.Range(minSouls, maxSouls + 1);


        for (int i = 0; i < soulCount; i++)
        {
            // Instanciar el prefab del sistema de partículas
            GameObject soulParticleSystem = Instantiate(soulPrefab, transform.position, Quaternion.identity);
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
