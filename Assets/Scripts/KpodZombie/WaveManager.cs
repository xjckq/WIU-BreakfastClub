using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> waves;
    public KPodZombiesSpawner spawner;
    private int currentWaveIndex = -1;
    private int spawnedEnemies = 0;
    private int enemiesKilled = 0;
    public TextMeshProUGUI waveText;
    public GameObject menuPanel;
    public GameObject GameUI;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverMessageText;
    //public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI wavesSurvivedText;
    void Start()
    {
        Time.timeScale = 1f;
        //StartCoroutine(WaitForNextWave(2f));
        //StartNextWave();
        menuPanel.SetActive(true);
        GameUI.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Only allow the cheat if a wave is currently active
            if (currentWaveIndex >= 0 && currentWaveIndex < waves.Count)
            {
                // Calculate how many more enemies need to be "killed" to complete the wave
                int remainingEnemies = waves[currentWaveIndex].totalEnemiesToSpawn - enemiesKilled;

                // Log a message to the console to confirm the cheat
                Debug.Log("Cheat activated! Skipping wave " + (currentWaveIndex + 1));

                // Call the EnemyKilled method for each remaining enemy
                // This will trigger the wave completion logic
                for (int i = 0; i < remainingEnemies; i++)
                {
                    EnemyKilled();
                }
            }
        }
    }
    public void StartGame()
    {
        menuPanel.SetActive(false);
        GameUI.SetActive(true);
        //StartNextWave();
        StartCoroutine(WaitForNextWave(0f));
    }
    public void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            spawnedEnemies = 0;
            enemiesKilled = 0;

            Debug.Log("Starting wave " + (currentWaveIndex + 1) + " of " + waves.Count);
            if (waveText != null)
            {
                waveText.text = "Wave " + (currentWaveIndex + 1) + " / " + waves.Count;
            }
            StartCoroutine(SpawnWaveCoroutine(waves[currentWaveIndex]));
        }
        else
        {
            EndGame(true);
            if (waveText != null)
            {
                waveText.text = "All Waves Completed!";
            }
            Debug.Log("All waves completed!");
        }
    }

    private IEnumerator SpawnWaveCoroutine(WaveData wave)
    {
        for (int i = 0; i < wave.totalEnemiesToSpawn; i++)
        {
            if (wave.zombiePrefabs.Length > 0)
            {
                GameObject zombieToSpawn = wave.zombiePrefabs[Random.Range(0, wave.zombiePrefabs.Length)];
                if (spawner != null)
                {
                    spawner.SpawnEnemyAtRandomPoint(zombieToSpawn);
                    spawnedEnemies++;
                }
            }
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
    public void EnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("Enemies killed in current wave: " + enemiesKilled);
        if (enemiesKilled >= waves[currentWaveIndex].totalEnemiesToSpawn)
        {
            Debug.Log("Wave " + (currentWaveIndex + 1) + " completed!");
            if (waveText != null)
            {
                waveText.text = "Wave " + (currentWaveIndex + 1) + " Completed!";
            }
            StartCoroutine(WaitForNextWave(2f));
        }
    }
    private IEnumerator WaitForNextWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentWaveIndex + 1 < waves.Count)
        {
            int nextWaveNumber = currentWaveIndex + 2;
            for (int i = 3; i > 0; i--)
            {
                if (waveText != null)
                {
                    waveText.text = "Wave " + nextWaveNumber + " Starting in " + i + "...";
                }
                yield return new WaitForSeconds(1f);
            }
        }
        StartNextWave();
    }
    public void EndGame(bool isWon)
    {
        Time.timeScale = 0f;
        GameUI.SetActive(false);
        gameOverPanel.SetActive(true);
        if (isWon)
        {
            gameOverMessageText.text = "You Win!";
            wavesSurvivedText.text = "Waves Survived: " + waves.Count.ToString();
            QuestManager.Instance.MinigameCompleted(QuestData.MinigameType.Kpod);
        }
        else
        {
            gameOverMessageText.text = "Game Over";
            currentWaveIndex = Mathf.Max(0, currentWaveIndex);
        }
       // enemiesKilledText.text = "Enemies Killed: " + enemiesKilled.ToString();
        wavesSurvivedText.text = "Waves Survived: " + currentWaveIndex.ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
