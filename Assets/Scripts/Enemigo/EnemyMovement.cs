using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;

    [HideInInspector]
    public GameObject target; 

    private SpriteRenderer spriteRenderer;
    private Vector3 networkPosition; // Para interpolar la posición en clientes

    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PhotonNetwork.IsMasterClient)
        {
            UpdateTarget(); // Solo el servidor actualiza el objetivo
        }
    }

    private void Update()
    {
        UpdateTarget();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsPlayer();
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            // Interpolación para suavizar el movimiento
            transform.position = Vector3.MoveTowards(transform.position, networkPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateTarget()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // Buscar al jugador más cercano
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        foreach (GameObject player in GameplayManager.instance.playersInGame)
        {
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }
        }

        target = closestPlayer;
        GetComponent<PhotonView>().RPC("SetTarget", RpcTarget.Others, target.GetPhotonView().ViewID);
    }

    [PunRPC]
    private void SetTarget(int targetViewID)
    {
        target = PhotonView.Find(targetViewID)?.gameObject;
    }


    private void MoveTowardsPlayer()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;

        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
    }
}
