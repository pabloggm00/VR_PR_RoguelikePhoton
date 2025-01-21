using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemyMovement : MonoBehaviourPunCallbacks
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;

    [HideInInspector]
    public Transform target;

    public SpriteRenderer spriteRenderer;
    private Vector3 networkPosition; // Para interpolar la posición en clientes


    // RPC para actualizar posición en clientes
    [PunRPC]
    private void UpdatePosition(Vector3 position)
    {
        networkPosition = position;
    }

    private void Start()
    {
        UpdateTarget();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateTarget();

            if (target != null)
            {
                MoveTowardsTarget();
            }
        }
    }

    private void UpdateTarget()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        target = PlayerManager.instance.FindNearestPlayer(transform);
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    [PunRPC]
    public void UpdateEnemyFlip(bool flipRight)
    {
        spriteRenderer.flipX = !flipRight; // Flipa al enemigo también
    }
}
