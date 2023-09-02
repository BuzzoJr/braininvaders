using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MothershipController : MonoBehaviour
{
    private bool stateIsPlaying;
    public GameObject projectilePrefab; // The projectile prefab to be instantiated
    public List<AudioClip> audioClips;
    private AudioSource audioSource;
    private int audioIndex = 0;
    private float shootInterval = 0.1f; // Time interval between each shot
    private int numProjectiles = 10; // Number of projectiles to be shot in the cone pattern
    private float coneAngle = 80f; // Angle of the cone pattern in degrees
    private float repetitions = 1f; // Times it makes the pattern
    private float angleStep;
    private float startAngle;
    [SerializeField]private int health = 100;
    [SerializeField]private Slider slider;

    Vector3 targetPosition;
    bool isMoving = false;
    float idleStartingY;
    float moveUp = 1f;
    float idleTimer = 0f; // Updated variable
    float idleDuration = 1f; // Duration of each idle animation cycle
    float speed = 0.3f;

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        audioSource = GetComponent<AudioSource>();
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
        idleStartingY = transform.position.y;
        slider.value = health;
        StartCoroutine(MoveBoss());
    }

    IEnumerator MoveBoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            float randomX = Random.Range(-13f, 13f);
            float randomY = Random.Range(6f, 10f);

            targetPosition = new Vector3(randomX, randomY, 0f);
            isMoving = true;

            float startTime = Time.time;
            float duration = 2f; // Duration for the boss to reach the target position

            Vector3 startPosition = transform.position;

            while (Time.time - startTime < duration)
            {
                float timeRatio = (Time.time - startTime) / duration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, timeRatio);

                yield return null;
            }

            transform.position = targetPosition;
            isMoving = false;
            idleStartingY = transform.position.y;
            idleTimer = 0.5f;
            speed = 0.3f;

            // Shoot projectiles when the boss stops moving
            yield return StartCoroutine(ShootProjectiles());
        }
    }

    void Update()
    {
        if (!isMoving || !stateIsPlaying)
        {
            // Calculate the idle position using Vector3.Lerp
            float idleRatio = idleTimer / idleDuration;
            float idleMovement = Mathf.Lerp(-speed, speed, idleRatio);

            // Increment the idleTimer
            float idlePositionY = idleStartingY + (idleMovement * moveUp);
            Vector3 newPosition = transform.position;
            newPosition.y = idlePositionY;

            transform.position = newPosition;

            idleTimer += Time.deltaTime;

            // Reset the idleTimer and moveUp flag if the idle duration is reached
            if (idleTimer >= idleDuration)
            {
                idleTimer = 0f;
                speed *= -1f;
            }
        }
    }

    IEnumerator ShootProjectiles()
    {
        if(stateIsPlaying)
        {
            int pattern = Random.Range(1, 4);

            if(pattern == 1) //3 ROTATING
            {
                shootInterval = 0.1f;
                numProjectiles = 30; 
                coneAngle = 720f; 
                angleStep = coneAngle / (numProjectiles - 1);
                startAngle = -coneAngle / 2f;
                for (int i = 0; i < numProjectiles; i++)
                {
                    Quaternion rotation = Quaternion.Euler(0f, 0f, startAngle + (angleStep * i));
                    Quaternion rotation2 = Quaternion.Euler(0f, 0f, startAngle + (angleStep * (i + numProjectiles/1.5f)));
                    Quaternion rotation3 = Quaternion.Euler(0f, 0f, startAngle + (angleStep * (i + numProjectiles/3f)));
                    Instantiate(projectilePrefab, transform.position, rotation);
                    Instantiate(projectilePrefab, transform.position, rotation2);
                    Instantiate(projectilePrefab, transform.position, rotation3);
                    PlaySound();
                    yield return new WaitForSeconds(shootInterval);
                }
            }else if(pattern == 2) // 2 CONE
            { 
            
                shootInterval = 0.15f;
                numProjectiles = 8;
                coneAngle = 150f;
                repetitions = 2;
                angleStep = coneAngle / (numProjectiles - 1);
                startAngle = -coneAngle / 2f;
                for(int n = 0; n < repetitions; n++)
                {
                    for (int i = 0; i < numProjectiles; i++)
                    {
                        Quaternion rotationLeft = Quaternion.Euler(0f, 0f, -startAngle - (angleStep * i));
                        Quaternion rotationRight = Quaternion.Euler(0f, 0f, startAngle + (angleStep * i));
                        Vector3 pointRight = new Vector3(transform.position.x + 2f, transform.position.y -1, transform.position.z);
                        Vector3 pointLeft = new Vector3(transform.position.x - 2f, transform.position.y -1, transform.position.z);
                        Instantiate(projectilePrefab, pointRight, rotationRight);
                        Instantiate(projectilePrefab, pointLeft, rotationLeft);
                        yield return new WaitForSeconds(shootInterval);
                        PlaySound();
                    }
                    yield return new WaitForSeconds(1f);
                }
            }else // CONE
            {
                shootInterval = 0.6f;
                numProjectiles = 11;
                coneAngle = 150f;
                repetitions = 3;
                angleStep = coneAngle / (numProjectiles - 1);
                startAngle = -coneAngle / 2f;
                for(int n = 0; n < repetitions; n++)
                {
                    for (int i = 0; i < numProjectiles; i++)
                    {
                        Quaternion rotation = Quaternion.Euler(0f, 0f, startAngle + (angleStep * i));
                        Instantiate(projectilePrefab, transform.position, rotation);

                    }
                    PlaySound();
                    yield return new WaitForSeconds(shootInterval);
                }
            }
            audioIndex = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            health -= 5;
            slider.value = health;
            if(health <= 0)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Victory);
            }
        }
    }

    private void PlaySound(){
        audioSource.PlayOneShot(audioClips[audioIndex]);
        if(audioIndex >= 2){
            audioIndex = 0;
        }else{
            audioIndex++;
        }
    }
}
