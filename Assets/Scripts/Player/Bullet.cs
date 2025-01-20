using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
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

    private void Update()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemigo))
        {
            enemigo.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage, type);
        }

        // Desactivar la bala tras colisionar
        //GetComponent<PhotonView>().RPC("DeactivateBullet", RpcTarget.AllBuffered);
        DestroyBullet();
    }

    [PunRPC]
    public void DeactivateBullet()
    {
        gameObject.SetActive(false); // Desactiva la bala
    }

    private void DestroyBullet()
    {
        if (photonView.IsMine) // Solo el propietario puede destruir la bala
        {
            PhotonNetwork.Destroy(gameObject); // Destruye la bala y sincroniza
        }
    }
}
