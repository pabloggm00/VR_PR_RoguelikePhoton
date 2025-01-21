using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public int minEnemies = 1;
    public int maxEnemies = 3;

    [Header("Lista de Enemigos")]
    public List<GameObject> regularEnemies;

    [Header("Rondas y Progresión")]
    public int roundsUntilBoss = 3; 
    private int currentRound = 0;
    public float roundPauseDuration = 2f;

    [Header("Referencias de la Sala")]
    public RoomGenerator roomGenerator; // Referencia al generador de la sala
    public float wallMargin = 2f; // Distancia mínima de los enemigos a las paredes

    [HideInInspector]
    public GameObject player;
    private List<GameObject> activeEnemies = new List<GameObject>(); // Lista de enemigos vivos

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }


    private void Start()
    {
        if (roomGenerator == null)
        {
            Debug.LogWarning("RoomGenerator aún no configurado al inicio.");
        }
    }

    public void SetRoomGenerator(RoomGenerator generator)
    {
        roomGenerator = generator;
        Debug.Log("RoomGenerator configurado correctamente.");
    }

    [PunRPC]
    public void SpawnEnemigos()
    {

        if (!PhotonNetwork.IsMasterClient) return;

        if (roomGenerator == null)
        {
            Debug.LogError("RoomGenerator no está configurado. Aborta el spawn.");
            return;
        }

        currentRound++;

        int enemyCount = Random.Range(minEnemies, maxEnemies);
        Debug.Log(enemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            // Determina el prefab del enemigo: jefe o enemigo regular.
            GameObject enemyPrefab =  regularEnemies[Random.Range(0, regularEnemies.Count)];

            // Genera una posición válida para el spawn.
            Vector2 spawnPosition = GetValidSpawnPosition();

            // Instancia y configura el enemigo.
            GameObject enemyObject = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPosition, Quaternion.identity);

            // Asigna el primer jugador como target inicial (se actualiza más tarde).
            enemyObject.GetComponent<EnemyMovement>().target = PlayerManager.instance.playersInGame[0].transform;

            // Añade el enemigo a la lista activa.
            activeEnemies.Add(enemyObject);
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        float halfWidth = roomGenerator.width / 2f;
        float halfHeight = (roomGenerator.height + 3) / 2f;

        float x = Random.Range(-halfWidth + wallMargin, halfWidth - wallMargin - 3);
        float y = Random.Range(0, halfHeight - wallMargin); // Solo en la mitad superior

        return new Vector2(x, y);
    }

    private void HandleEnemyDeath(int viewID)
    {

        PhotonView enemyView = PhotonView.Find(viewID);
        if (enemyView != null)
        {
            GameObject enemyObject = enemyView.gameObject;

            // Eliminar el enemigo solo si eres el MasterClient
            if (PhotonNetwork.IsMasterClient)
            {
                activeEnemies.Remove(enemyObject);
                PhotonNetwork.Destroy(enemyView.gameObject);
            }
        }

        // Si no quedan enemigos, spawnear la siguiente ronda
        if (activeEnemies.Count == 0)
        {
            StartCoroutine(HandleRoundPause());
        }
    }

    private IEnumerator HandleRoundPause()
    {

        // Mostrar mensaje o animación opcional aquí
        Debug.Log($"Ronda {currentRound} terminada. Pausa de {roundPauseDuration} segundos...");

        // Esperar la duración de la pausa
        yield return new WaitForSeconds(roundPauseDuration);

        if (PhotonNetwork.IsMasterClient)
        {
            // Spawnear la siguiente ronda
            GetComponent<PhotonView>().RPC("SpawnEnemigos", RpcTarget.All);
        }
 

    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el área de spawn válida en el editor
        if (roomGenerator != null)
        {
            float halfWidth = roomGenerator.width / 2f;
            float halfHeight = (roomGenerator.height + 3) / 2f;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(
                new Vector3(0, halfHeight / 2, 0),
                new Vector3(roomGenerator.width - 2 * wallMargin, halfHeight - wallMargin - 3, 0)
            );

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                new Vector3(0, 0, 0),
                new Vector3(roomGenerator.width, roomGenerator.height, 0)
            );

        }
    }
}
