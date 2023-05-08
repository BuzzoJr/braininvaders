using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationTime;
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;
    public System.Action<Invader> killed;
    public ExplosionController explosion;
    private bool stateIsPlaying;
    public int scoreValue = 10;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        Scene currentScene = SceneManager.GetActiveScene();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (currentScene.name == "Level2 - Autism")
        {
            _spriteRenderer.color = new Color(0.73f, 0.73f, 0.73f);
        }
        else if (currentScene.name == "Level3 - ADHD")
        {
            //StartCoroutine(ChangeColor());
            _spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
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

    IEnumerator ChangeColor()
    {
        while (true)
        {
            if (stateIsPlaying)
            {
                // Generate random values for red, green, and blue color components
                

                float red = Random.Range(0f, 1f);
                float green = Random.Range(0f, 1f);
                float blue = Random.Range(0f, 1f);

                // Set the SpriteRenderer's color to the resulting color
                _spriteRenderer.color = new Color(red, green, blue);

                // Wait for 2 seconds before changing the color again
            }
            yield return new WaitForSeconds(2f);
        }
    }

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        if (stateIsPlaying)
        {
            _animationFrame++;

            if(_animationFrame >= this.animationSprites.Length){
                _animationFrame = 0;
            }
            _spriteRenderer.sprite = this.animationSprites[_animationFrame];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            this.killed.Invoke(this);
            this.gameObject.SetActive(false);
        }
    }
}
