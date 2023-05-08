using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InvadersGrid : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public int amountKilled {get; private set; }
    public int totalInvaders => rows * columns;
    public int amountAlive => totalInvaders - amountKilled;
    public float percentKilled => (float)amountKilled / (float)totalInvaders;
    public float missileAttackRate = 1.5f;
    public ProjectileController missilePrefab;
    public TextMeshProUGUI scoreText;
    private int totalScore = 0;
    private Vector3 _direction = Vector3.right;
    private bool stateIsPlaying;
    private Scene currentScene;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        currentScene = SceneManager.GetActiveScene();
        for (int row = 0; row < this.rows; row++){
            float width = 2f * (this.columns -1);
            float height = 2f * (this.rows -1);
            Vector2 centering = new Vector2(-width/2, -height/2);  
            Vector3 rowPosition = new Vector3(centering.x, centering.y + row * 2f, 0);
            for(int col = 0; col < this.columns; col++){
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col* 2f;
                invader.transform.localPosition = position;
            }
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


    void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileAttackRate, missileAttackRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (stateIsPlaying)
        {
            this.transform.position += _direction * this.speed.Evaluate(percentKilled) * Time.deltaTime;

            Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
            Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

            foreach (Transform invader in this.transform)
            {
                if(!invader.gameObject.activeInHierarchy)
                {
                    continue;
                }
                if(_direction == Vector3.right && invader.position.x >= (rightEdge.x -1f))
                {
                    AdvanceRow();
                }else if(_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
                {
                    AdvanceRow();
                }
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1f;
        Vector3 position = this.transform.position;
        position.y -= 1f;
        this.transform.position = position;
    }

    private void InvaderKilled(Invader deadInvader)
    {
        int scoreAwarded = deadInvader.scoreValue;
        if (currentScene.name == "Level3 - ADHD")
        {
            scoreAwarded *= 927;
        }
        totalScore += scoreAwarded;
        scoreText.text = "Score: " + totalScore;
        amountKilled++;
        if(amountKilled >= totalInvaders)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Victory);
        }
    }

    public void MissileAttack()
    {
        if (stateIsPlaying)
        {
            foreach (Transform invader in this.transform)
            {
                if(!invader.gameObject.activeInHierarchy)
                {
                    continue;
                }
               if(Random.value < (1f / (float)amountAlive))
               {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
               }
            }
        }
    }
}
