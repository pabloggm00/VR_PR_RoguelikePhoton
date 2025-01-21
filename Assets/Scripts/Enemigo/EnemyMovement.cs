using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class EnemyMovement : MonoBehaviourPunCallbacks
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;

    [HideInInspector]
    public Transform target;

    public SpriteRenderer spriteRenderer;
    private Vector3 networkPosition; // Para interpolar la posición en clientes



    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FindClosestPlayer();
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void FindClosestPlayer()
    {
        GameObject[] players = GameplayManager.instance.players.ToArray();
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = player.transform;
            }
        }
    }

    // RPC para actualizar posición en clientes
    /*[PunRPC]
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
        Debug.Log(target);
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
    }*/
}
