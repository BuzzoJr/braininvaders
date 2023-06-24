using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private AudioSource dialogSound;
    private int index;
    private bool wonLevel = false;

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        dialogSound = GetComponent<AudioSource>();

    }

    void Start()
    {
        textComponent.text = string.Empty;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameManager.GameState state)
    {
        if(state.ToString() == "Victory")
        {
            wonLevel = true;
        }
        if (state.ToString() == "Dialog" || state.ToString() == "Victory")
        {
            gameObject.SetActive(true);
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index]) 
            { 
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogSound.Stop();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialog()
    {
        index = 0;
    }

    IEnumerator TypeLine() //Escreve a linha com base no tempo setado
    {
        yield return new WaitForSeconds(0.05f);
        dialogSound.Play();
        foreach (char c in lines[index].ToCharArray()) 
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        dialogSound.Stop();
    }

    void NextLine() //Pula para proxima linha ou fecha caixa de di�logo
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            System.Array.Resize(ref lines, 0);
            textComponent.text = string.Empty;
            index = 0;
            gameObject.SetActive(false);
            if (!wonLevel)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
            }
            else
            {
                int currentScene = SceneManager.GetActiveScene().buildIndex;
                if(currentScene == 6){
                    SceneManager.LoadScene(0);
                }else{
                    SceneManager.LoadScene(currentScene + 1);
                }
            }
        }
    }
}
