using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemyMovement : MonoBehaviourPunCallbacks
{
    [Header("Movimiento")]
    public float moveSpeed = 2f;

    [HideInInspector]
    public GameObject target;

    private SpriteRenderer spriteRenderer;
    private Vector3 networkPosition; // Para interpolar la posici�n en clientes

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
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateTarget(); // Solo el servidor actualiza el objetivo
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // Solo el MasterClient mueve al enemigo
            if (target != null)
            {
                MoveTowardsPlayer();
                GetComponent<PhotonView>().RPC("UpdatePosition", RpcTarget.Others, transform.position);
            }
        }
        else
        {
            // Clientes interpolan hacia la posici�n enviada
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * moveSpeed);
        }
    }

    private void FixedUpdate()
    {
        
    }

    // RPC para actualizar posici�n en clientes
    [PunRPC]
    private void UpdatePosition(Vector3 position)
    {
        networkPosition = position;
    }

    private void UpdateTarget()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        // Encuentra el jugador m�s cercano
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

        // Actualiza el target si cambi�
        if (closestPlayer != target)
        {
            target = closestPlayer;
            int targetViewID = target != null ? target.GetComponent<PhotonView>().ViewID : -1;
            GetComponent<PhotonView>().RPC("SetTarget", RpcTarget.All, targetViewID);
        }
    }

    [PunRPC]
    private void SetTarget(int targetViewID)
    {
        // Recupera el GameObject a partir del ViewID
        if (targetViewID != -1)
        {
            target = PhotonView.Find(targetViewID)?.gameObject;
        }
        else
        {
            target = null;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (target == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        // Ajusta la orientaci�n del sprite seg�n la direcci�n
        if (direction.x < 0)
        {
            photonView.RPC("UpdateEnemyFlip", RpcTarget.AllBuffered, false); // Flipa el sprite a la izquierda
        }
        else if (direction.x > 0)
        {
            photonView.RPC("UpdateEnemyFlip", RpcTarget.AllBuffered, true); // Flipa el sprite a la derecha
        }
    }

    [PunRPC]
    public void UpdateEnemyFlip(bool flipRight)
    {
        Debug.Log(spriteRenderer);
        spriteRenderer.flipX = !flipRight; // Flipa al enemigo tambi�n
    }
}
