using System.Collections;
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
}