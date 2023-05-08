using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicController : MonoBehaviour
{
    public AudioClip[] audioClips;
    private int currentClipIndex = 0;
    private float timeBetweenClips = 1f;
    private AudioSource audioSource;
    private int loopCount = 0;
    private float timeIncreaseMod = 0;
    private bool stateIsPlaying;

    // Start is called before the first frame update
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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name == "Level2 - Autism" || SceneManager.GetActiveScene().name == "Level1 - Base")
        {
            timeIncreaseMod = 0.85f;
        }else if (SceneManager.GetActiveScene().name == "Level3 - ADHD")
        {
            timeIncreaseMod = 1f;
            timeBetweenClips = 0.378f;
        }
        StartCoroutine(PlayClips());
    }

    // Update is called once per frame
    IEnumerator PlayClips()
    {
        while (true)
        {
            if (stateIsPlaying)
            {
                audioSource.PlayOneShot(audioClips[currentClipIndex]);
                currentClipIndex++;

                if (currentClipIndex >= audioClips.Length)
                {
                    loopCount++;
                    currentClipIndex = 0;
                    if (loopCount == 4)
                    {
                        timeBetweenClips *= timeIncreaseMod; // increase speed by 10% every loop
                        timeBetweenClips = Mathf.Clamp(timeBetweenClips, 0.07f, 1f); // limit the minimum value to 0.07
                        loopCount = 0;
                    }
                }
            }
            yield return new WaitForSeconds(timeBetweenClips);
        }
    }
}
