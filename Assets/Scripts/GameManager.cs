using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyOne;
    public GameObject enemy;
    public GameObject cloud;
    public GameObject powerup;
    public GameObject LifePowerUp;
    public GameObject sideEnemy;
    public GameObject sideSideEnemy;

    public AudioClip powerUp;
    public AudioClip powerDown;

    public int cloudSpeed;

    private bool isPlayerAlive;
    private int score;
    private int playerLives = 3;  // Player's initial lives

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI restartText;
    public TextMeshProUGUI powerupText;
    public TextMeshProUGUI livesText;  // Reference to lives display

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, transform.position, Quaternion.identity);
        InvokeRepeating("CreateEnemyOne", 1f, 3f);
        InvokeRepeating("CreateEnemy", 1f, 3f);
        InvokeRepeating("CreateSideEnemy", 1f, 6f);
        InvokeRepeating("CreateSideSideEnemy", 3f, 3f);
        InvokeRepeating("CreateLifePowerUp", 3f, 9f);
        StartCoroutine(CreatePowerup());
        CreateSky();
        score = 0;
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + playerLives;  // Initialize lives display
        isPlayerAlive = true;
        cloudSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Restart();
    }

    void CreateEnemyOne()
    {
        Instantiate(enemyOne, new Vector3(Random.Range(-9f, 9f), 7.5f, 0), Quaternion.Euler(0, 0, 180));
    }

    void CreateEnemy()
    {
        Instantiate(enemy, new Vector3(Random.Range(-9f, 9f), 7.5f, 0), Quaternion.identity);
    }

    void CreateSideEnemy()
    {
        Instantiate(sideEnemy, new Vector3(24f, Random.Range(1f, 9f), 0), Quaternion.identity);
        Debug.Log("Side Enemy Spawned");
    }

    void CreateSideSideEnemy()
    {
        Instantiate(sideSideEnemy, new Vector3(-24f, Random.Range(1f, 9f), 0), Quaternion.identity);
        Debug.Log("Side Side Enemy Spawned");
    }

    IEnumerator CreatePowerup()
    {
        Instantiate(powerup, new Vector3(Random.Range(-9f, 9f), 7.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        StartCoroutine(CreatePowerup());
    }

    void CreateLifePowerUp()
    {
        Instantiate(LifePowerUp, new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0), Quaternion.identity);
    }

    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 cloudPosition = new Vector3(Random.Range(-9f, 9f), Random.Range(5f, 10f), 0);  // Random y position for variety
            Instantiate(cloud, cloudPosition, Quaternion.identity);
        }
    }

    public void EarnScore(int newScore)
    {
        score += newScore;
        scoreText.text = "Score: " + score;
    }

    public void LoseLife()
    {
        playerLives--;  // Decrease lives by 1
        livesText.text = "Lives: " + playerLives;  // Update the UI text to show remaining lives

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        isPlayerAlive = false;
        CancelInvoke();
        CancelInvoke("CreateEnemy");  // Stops enemy spawning
        gameOverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        cloudSpeed = 0;
    }

    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isPlayerAlive)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void UpdatePowerupText(string whichPowerup)
    {
        powerupText.text = whichPowerup;
    }

    public void PlayPowerUp()
    {
        AudioSource.PlayClipAtPoint(powerUp, Camera.main.transform.position);
    }

    public void PlayPowerDown()
    {
        AudioSource.PlayClipAtPoint(powerDown, Camera.main.transform.position);
    }
}