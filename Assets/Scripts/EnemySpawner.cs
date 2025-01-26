using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public TextMeshProUGUI statusText; // Unified TextMeshProUGUI for wave count and Game Over message
    public PlayerMovement player; // Reference to the PlayerMovement script

    public int waveCount = 1;
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;
    public int enemyCount;
    public GameObject[] enemyPrefabs; // Array to hold the three enemy prefabs
    public Transform[] spawnPoints; // Array of spawn points in the scene
    public List<GameObject> spawnedEnemies = new List<GameObject>(); // List to track spawned enemies

    bool waveIsDone = true;

    GameObject enemyType1;
    GameObject enemyType2;

    void Update()
    {
        // Update the wave count text to display just the number
        if (player != null && player.GetPlayerHealth() > 0)
        {
            statusText.text = waveCount.ToString(); // Only show the wave number
        }

        // Stop spawning if the player is dead
        if (player == null || player.GetPlayerHealth() <= 0)
        {
            GameOver();
            return;
        }

        if (waveIsDone)
        {
            StartCoroutine(waveSpawner());
        }
    }

    IEnumerator waveSpawner()
    {
        waveIsDone = false;

        // Randomly pick two enemy types from the enemyPrefabs array
        SelectRandomEnemyTypes();

        for (int i = 0; i < enemyCount; i++)
        {
            // Randomly decide which of the two selected types to spawn
            GameObject selectedEnemy = Random.Range(0, 2) == 0 ? enemyType1 : enemyType2;

            // Randomly select a spawn point from the array
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Spawn the enemy at the selected spawn point
            GameObject enemyClone = Instantiate(selectedEnemy, selectedSpawnPoint.position, Quaternion.identity);
            spawnedEnemies.Add(enemyClone); // Add spawned enemy to the list

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate = Mathf.Max(0.1f, spawnRate - 0.1f); // Ensure spawn rate does not go negative
        enemyCount += 3;
        waveCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }

    void SelectRandomEnemyTypes()
    {
        // Randomly pick two distinct indices
        int index1 = Random.Range(0, enemyPrefabs.Length);
        int index2;

        do
        {
            index2 = Random.Range(0, enemyPrefabs.Length);
        } while (index2 == index1); // Ensure the second index is different

        enemyType1 = enemyPrefabs[index1];
        enemyType2 = enemyPrefabs[index2];
    }

    void GameOver()
    {
        // Stop spawning
        waveIsDone = false;

        // Destroy all spawned enemies
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();

        // Display Game Over message using the same text object
        if (statusText != null)
        {
            statusText.text = "Game Over!";
            statusText.color = Color.red; // Optionally, change the color for Game Over
        }

        InGameSceneManager.Instance.GameOverScreen();

        Debug.Log("Game Over!");
    }
}
