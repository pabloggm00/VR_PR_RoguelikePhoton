using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public Rect spawnArea; 
    public int minEnemies = 1;
    public int maxEnemies = 3;

    [Header("Lista de Enemigos")]
    public List<GameObject> regularEnemies; 
    public GameObject bossEnemy;

    [Header("Rondas y Progresión")]
    public int roundsUntilBoss = 3; 
    private int currentRound = 0;
    public float roundPauseDuration = 2f;

    [Header("Referencias de la Sala")]
    public RoomGenerator roomGenerator; // Referencia al generador de la sala
    public float wallMargin = 2f; // Distancia mínima de los enemigos a las paredes

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

    public void SpawnEnemigos()
    {
        currentRound++;

        if (currentRound % roundsUntilBoss == 0) //Cada x rondas, aparece un jefe
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
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.GetComponent<EnemyMovement>().target = player;
        activeEnemies.Add(enemy); 
    }

    private Vector2 GetValidSpawnPosition()
    {
        float halfWidth = roomGenerator.width / 2f;
        float halfHeight = roomGenerator.height / 2f;

        float x = Random.Range(-halfWidth + wallMargin, halfWidth - wallMargin);
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

        // Mostrar mensaje o animación opcional aquí
        Debug.Log($"Ronda {currentRound} terminada. Pausa de {roundPauseDuration} segundos...");

        // Esperar la duración de la pausa
        yield return new WaitForSeconds(roundPauseDuration);

        // Spawnear la siguiente ronda
        SpawnEnemigos();
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el área de spawn válida en el editor
        if (roomGenerator != null)
        {
            float halfWidth = roomGenerator.width / 2f;
            float halfHeight = roomGenerator.height / 2f;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(
                new Vector3(0, halfHeight / 2, 0),
                new Vector3(roomGenerator.width - 2 * wallMargin, halfHeight - wallMargin, 0)
            );
        }
    }
}
