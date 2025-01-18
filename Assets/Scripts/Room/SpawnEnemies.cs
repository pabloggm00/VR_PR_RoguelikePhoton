using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Configuraci�n de Spawn")]
    public int minEnemies = 1;
    public int maxEnemies = 3;

    [Header("Lista de Enemigos")]
    public List<GameObject> regularEnemies; 
    public GameObject bossEnemy;

    [Header("Rondas y Progresi�n")]
    public int roundsUntilBoss = 3; 
    private int currentRound = 0;
    public float roundPauseDuration = 2f;

    [Header("Referencias de la Sala")]
    public RoomGenerator roomGenerator; // Referencia al generador de la sala
    public float wallMargin = 2f; // Distancia m�nima de los enemigos a las paredes

    [HideInInspector]
    public GameObject player;
    private List<Enemy> activeEnemies = new List<Enemy>(); // Lista de enemigos vivos

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
            Debug.LogWarning("RoomGenerator a�n no configurado al inicio.");
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
        if (roomGenerator == null)
        {
            Debug.LogError("RoomGenerator no est� configurado. Aborta el spawn.");
            return;
        }

        currentRound++;

        if (currentRound % roundsUntilBoss == 0)
        {
            SpawnEnemy(bossEnemy);
        }
        else
        {
            int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemyToSpawn = regularEnemies[Random.Range(0, regularEnemies.Count)];
                SpawnEnemy(enemyToSpawn);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = GetValidSpawnPosition();
        GameObject enemyObject = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPosition, Quaternion.identity);
        enemyObject.GetComponent<EnemyMovement>().target = GameplayManager.instance.playersInGame[0]; // Default, actualizar� despu�s.
        activeEnemies.Add(enemyObject.GetComponent<Enemy>());
    }

    private Vector2 GetValidSpawnPosition()
    {
        float halfWidth = roomGenerator.width / 2f;
        float halfHeight = (roomGenerator.height + 3) / 2f;

        float x = Random.Range(-halfWidth + wallMargin, halfWidth - wallMargin - 3);
        float y = Random.Range(0, halfHeight - wallMargin); // Solo en la mitad superior

        return new Vector2(x, y);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        activeEnemies.Remove(enemy);

        // Si no quedan enemigos, spawnear la siguiente ronda
        if (activeEnemies.Count == 0)
        {
            StartCoroutine(HandleRoundPause());
        }
    }

    private IEnumerator HandleRoundPause()
    {

        // Mostrar mensaje o animaci�n opcional aqu�
        Debug.Log($"Ronda {currentRound} terminada. Pausa de {roundPauseDuration} segundos...");

        // Esperar la duraci�n de la pausa
        yield return new WaitForSeconds(roundPauseDuration);

        // Spawnear la siguiente ronda
        GetComponent<PhotonView>().RPC("SpawnEnemigos", RpcTarget.All);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el �rea de spawn v�lida en el editor
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
