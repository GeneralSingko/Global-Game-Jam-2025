/*using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public TextMeshProUGUI waveCountText;
    public TextMeshProUGUI gameOverText; // Reference to a Game Over text or UI element
    public PlayerMovement player; // Reference to the PlayerMovement script

    int waveCount = 1;
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;
    public int enemyCount;
    public GameObject enemy;
    public List<GameObject> spawnedEnemies = new List<GameObject>(); // List to track spawned enemies

    bool waveIsDone = true;

    void Update()
    {
        waveCountText.text = "Wave: " + waveCount.ToString();

        // Stop spawning if the player is dead
        if (player == null || player.GetPlayerHealth() <= 0)
        {
            GameOver();
            return;
        }

        if (waveIsDone == true)
        {
            StartCoroutine(waveSpawner());
        }

    }

    IEnumerator waveSpawner()
    {
        waveIsDone = false;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy);

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.1f;
        enemyCount += 3;
        waveCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
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

        // Display Game Over message
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over!";
            gameOverText.gameObject.SetActive(true); // Show the Game Over UI element
        }

        Debug.Log("Game Over!");
    }
}*/

using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public TextMeshProUGUI statusText; // Unified TextMeshProUGUI for wave count and Game Over message
    public PlayerMovement player; // Reference to the PlayerMovement script

    int waveCount = 1;
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;
    public int enemyCount;
    public GameObject[] enemyPrefabs; // Array to hold the three enemy prefabs
    public List<GameObject> spawnedEnemies = new List<GameObject>(); // List to track spawned enemies

    bool waveIsDone = true;

    GameObject enemyType1;
    GameObject enemyType2;

    void Update()
    {
        // Update the wave count text
        if (player != null && player.GetPlayerHealth() > 0)
        {
            statusText.text = "Wave: " + waveCount.ToString();
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

            GameObject enemyClone = Instantiate(selectedEnemy);
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

        Debug.Log("Game Over!");
    }
}
