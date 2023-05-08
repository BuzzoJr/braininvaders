using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileController : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public System.Action destroyed;
    private int objectLayer;
    private string layerName;
    private Scene currentScene;
    private SpriteRenderer spriteRenderer;
    private bool stateIsPlaying = true;
    // Start is called before the first frame update

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        objectLayer = gameObject.layer;
        layerName = LayerMask.LayerToName(objectLayer);
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;

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
        if(currentScene.name != "Level3 - ADHD" && layerName == "Laser")
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
        if(destroyed != null)
        {
        this.destroyed.Invoke();
        }
        Destroy(this.gameObject);
    }
}
