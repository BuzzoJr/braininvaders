using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangingText : MonoBehaviour
{
    public List<TMP_FontAsset> fontsList;
    public AudioSource sceneSound;
    private TextMeshProUGUI textMeshPro;
    private string alphabet = "FGH";
    private int fontIndex;
    private int index;
    private KeyCode currentKeyCode;
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
            StartCoroutine(GrowingSound());
            StartCoroutine(ChangeLetter());
        }
        else
        {
            stateIsPlaying = false;
        }
    }

    void Start()
    {
        textMeshPro = transform.GetComponent<TextMeshProUGUI>();
    }

    IEnumerator ChangeLetter()
    {
        while (true)
        {
            fontIndex = Random.Range(0, fontsList.Count);
            TMP_FontAsset font = fontsList[fontIndex];
            textMeshPro.font = font;

            
            index = Random.Range(0, alphabet.Length);
            textMeshPro.text = "Press " + alphabet[index];
            currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), alphabet[index].ToString());
            // Wait for 2 seconds before changing the color again
            yield return new WaitForSeconds(5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stateIsPlaying)
        {
            if (Input.GetKeyDown(currentKeyCode))
            {
                if(sceneSound.volume > 0.13f)
                {
                    sceneSound.volume -= 0.1f;
                }
            }

        }
        else
        {
            sceneSound.volume = 0.0f;
        }

    }

    IEnumerator GrowingSound()
    {
        while (true)
        {
            sceneSound.volume += 0.05f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
