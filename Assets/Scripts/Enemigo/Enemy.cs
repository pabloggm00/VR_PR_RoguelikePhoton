using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enemy : MonoBehaviour
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
    public delegate void EnemyDeathHandler(Enemy enemy);
    public static event EnemyDeathHandler OnEnemyDeath;

    private void Start()
    {
        currentHP = hP;

        damageEffect = GetComponent<DamageEffect>();
        damageEffect.Init(spriteRenderer);

        elements = new List<ElementSprite>();

        enemySpritesSettings.AgregarSprites(elements);

        elementCurrent = GetRandomElement();
        
        spriteRenderer.sprite = elementCurrent.sprite;
    }

    ElementSprite GetRandomElement()
    {
        int rnd = Random.Range(0, elements.Count);

        return elements[rnd];
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
        OnEnemyDeath?.Invoke(this); // Notificar al spawner
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
