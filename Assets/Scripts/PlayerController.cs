using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public ProjectileController laserPrefab;
    private bool _laserActive;
    private Scene currentScene;
    private bool stateIsPlaying;

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
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
                this.transform.position += Vector3.left * this.speed * Time.deltaTime;
            }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.position += Vector3.right * this.speed * Time.deltaTime;
            }else if (currentScene.name == "Level3 - ADHD" && Input.GetKey(KeyCode.S)) 
            {
                this.transform.position += Vector3.down * this.speed * Time.deltaTime;
            }
            else if (currentScene.name == "Level3 - ADHD" && Input.GetKey(KeyCode.W))
            {
                this.transform.position += Vector3.up * this.speed * Time.deltaTime;
            }

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if(!_laserActive)
                {
                Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
