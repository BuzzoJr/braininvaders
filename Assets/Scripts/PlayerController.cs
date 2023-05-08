using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public ProjectileController laserPrefab;
    public GameObject explosionPrefab;
    public GameObject lifeBar;
    private AudioSource explosionPlayer;
    private bool _laserActive;
    private Scene currentScene;
    private bool stateIsPlaying;
    private int lives = 3;
    private SpriteRenderer playerSpriteRenderer;
    private bool immortal = false;

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        explosionPlayer = GetComponent<AudioSource>();
    }
    void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameManager.GameState state)
    {
        if (state.ToString() == "Playing")
        {
            stateIsPlaying = true;
        }
        else
        {
            stateIsPlaying = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Level3 - ADHD")
        {
            speed *= 4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stateIsPlaying)
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x > -15f)
                {
                    this.transform.position += Vector3.left * this.speed * Time.deltaTime;
                }
            }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x < 15f)
                { 
                    this.transform.position += Vector3.right * this.speed * Time.deltaTime;
                }
            }
            //else if (currentScene.name == "Level3 - ADHD" && Input.GetKey(KeyCode.S)) 
            //{
            //    this.transform.position += Vector3.down * this.speed * Time.deltaTime;
            //}
            //else if (currentScene.name == "Level3 - ADHD" && Input.GetKey(KeyCode.W))
            //{
            //    this.transform.position += Vector3.up * this.speed * Time.deltaTime;
            //}

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if(!_laserActive)
                {
                Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        ProjectileController projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
        projectile.destroyed += LaserDestroyed;
        _laserActive = true;
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Invader")
        || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            if(lives > 0)
            {
                if (!immortal)
                {

                    lifeBar.transform.GetChild(lives - 1).gameObject.SetActive(false);
                    explosionPlayer.Play();
                    lives--;
                    immortal = true;
                    StartCoroutine(RespawnCoroutine());

                }
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        // play explosion animation
        var explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        if (currentScene.name == "Level2 - Autism")
        {
            explosion.GetComponent<SpriteRenderer>().color = new Color(0.73f, 0.73f, 0.73f);
        }
        playerSpriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.5f); // play for half a second
        explosion.SetActive(false);
        // move player to center of screen
        this.transform.position = new Vector3(0, transform.position.y, transform.position.z); ;
        // blink player for 2 seconds
        float blinkTime = Time.time + 2f;
        while (Time.time < blinkTime)
        {
            playerSpriteRenderer.enabled = !playerSpriteRenderer.enabled;
            yield return new WaitForSeconds(0.2f); // blink at a rate of 5 times per second
        }
        playerSpriteRenderer.enabled = true; // make sure the player is visible again
        immortal = false;
    }
}
