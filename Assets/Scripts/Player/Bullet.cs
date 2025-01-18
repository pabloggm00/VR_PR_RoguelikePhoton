using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private ElementType type;
    public SpriteRenderer spriteRenderer;


    [PunRPC]
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
            enemigo.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage, type);
        }
        
        // Colisión con cualquier objeto, desactiva la bala
        GetComponent<PhotonView>().RPC("DesactivateBullet", RpcTarget.All, null);
    }


    [PunRPC]
    public void DesactivateBullet()
    {
        gameObject.SetActive(false); // Desactivar la bala
    }
}
