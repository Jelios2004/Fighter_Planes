using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float speed;
    private float horizontalScreenLimit = 11.5f;
    private float verticalScreenLimit = 7.5f;
    private int lives;
    private int shooting;
    private bool hasShield;

    public GameManager gameManager;

    public GameObject bullet;
    public GameObject explosion;
    public GameObject thruster;
    public GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6f;
        lives = 3; // Initialize player's lives
        shooting = 1; // Initialize shooting type
        hasShield = false; // Player starts without a shield
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        if (transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (shooting)
            {
                case 1:
                    Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bullet, transform.position + new Vector3(-0.5f, 1, 0), Quaternion.identity);
                    Instantiate(bullet, transform.position + new Vector3(0.5f, 1, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bullet, transform.position + new Vector3(-0.5f, 1, 0), Quaternion.Euler(0, 0, 30f));
                    Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    Instantiate(bullet, transform.position + new Vector3(0.5f, 1, 0), Quaternion.Euler(0, 0, -30f));
                    break;
            }
        }
    }

    public void LoseALife()
    {
        if (!hasShield)
        {
            lives--; // Decrease player's lives
        }
        else
        {
            hasShield = false; // Disable shield
            shield.gameObject.SetActive(false);
        }

        gameManager.LoseLife(); // Update lives display in GameManager

        if (lives == 0)
        {
            gameManager.GameOver(); // Trigger game over
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        gameManager.PlayPowerDown();
        speed = 6f;
        thruster.gameObject.SetActive(false);
        gameManager.UpdatePowerupText("");
    }

    IEnumerator ShootingPowerDown()
    {
        yield return new WaitForSeconds(3f);
        gameManager.PlayPowerDown();
        shooting = 1;
        gameManager.UpdatePowerupText("");
    }

    public IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(3f);
        gameManager.PlayPowerDown();
        hasShield = false;
        shield.gameObject.SetActive(false);
        gameManager.UpdatePowerupText("");
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Powerup")
        {
            gameManager.PlayPowerUp();
            int powerupType = Random.Range(1, 5); // Randomly select a powerup type (1-4)
            switch (powerupType)
            {
                case 1:
                    speed = 9f;
                    gameManager.UpdatePowerupText("Picked up Speed!");
                    thruster.gameObject.SetActive(true);
                    StartCoroutine(SpeedPowerDown());
                    break;
                case 2:
                    shooting = 2;
                    gameManager.UpdatePowerupText("Picked up Double Shot!");
                    StartCoroutine(ShootingPowerDown());
                    break;
                case 3:
                    shooting = 3;
                    gameManager.UpdatePowerupText("Picked up Triple Shot!");
                    StartCoroutine(ShootingPowerDown());
                    break;
                case 4:
                    hasShield = true;
                    gameManager.UpdatePowerupText("Picked up Shield!");
                    shield.gameObject.SetActive(true);
                    StartCoroutine(ShieldPowerDown());
                    break;
            }
            Destroy(whatIHit.gameObject);
        }
    }
}
