using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileController : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public System.Action destroyed;
    public GameObject spriteMaskPrefab;
    private int objectLayer;
    private string layerName;
    private Scene currentScene;
    private SpriteRenderer spriteRenderer;
    private bool stateIsPlaying = true;
    private Collider2D thisCollider;
    private int isInMask = 0;
    private bool inBunker = false;
    // Start is called before the first frame update

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        objectLayer = gameObject.layer;
        layerName = LayerMask.LayerToName(objectLayer);
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        thisCollider = GetComponent<Collider2D>();

        if (currentScene.name == "Level2 - Autism")
        {
            if(layerName == "Missile")
            {
                spriteRenderer.color = new Color(0.73f, 0.73f, 0.73f, 0.5f);
            }
            else
            {
                spriteRenderer.color = new Color(0.73f, 0.73f, 0.73f);
            }
        }
        if(currentScene.name != "Level3 - ADHD" && currentScene.name != "Level5 - Depression" && layerName == "Laser")
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
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

    // Update is called once per frame
    void Update()
    {
        if (stateIsPlaying)
        {
            this.transform.position+= this.direction * this.speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.gameObject.layer == 12)
        {
            isInMask++;
        }
        else if (other.transform.gameObject.layer == 13)
        {
            inBunker = true;
            if (isInMask == 0)
            {
                if (destroyed != null)
                {
                    this.destroyed.Invoke();
                }
                CreateMask();
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (destroyed != null)
            {
                this.destroyed.Invoke();
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.transform.gameObject.layer == 12)
        {
            isInMask--;
            if (inBunker && isInMask == 0)
            {
                if (destroyed != null)
                {
                    this.destroyed.Invoke();
                }
                CreateMask();
                Destroy(this.gameObject);
            }
        }
        else if (other.transform.gameObject.layer == 13)
        {
            inBunker = false;
        } 
    }

    private void CreateMask()
    {
        Vector3 spawnPosition = transform.position;
        if (layerName == "Missile")
        {
            spawnPosition.y -= 0.20f;
        }
        else
        {
            spawnPosition.y -= 0.15f;
        }
        Instantiate(spriteMaskPrefab, spawnPosition, Quaternion.identity);
    }
}
