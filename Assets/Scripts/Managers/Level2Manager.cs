using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Level2Manager : MonoBehaviour
{
    public DialogController dialogController;
    public List<TMP_FontAsset> fontsList;
    public AudioSource sceneSound;
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI scoreText;
    public Volume postProcessingVolume;
    private Bloom bloomEffect;
    private string alphabet = "FGH";
    private int fontIndex;
    private int index;
    private KeyCode currentKeyCode;
    private bool stateIsPlaying;
    private int growingSoundLoop = 0;
    private Coroutine growingSoundCoroutine;
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
        if (state.ToString() == "Victory")
        {
            stateIsPlaying = false;
            StopCoroutine(growingSoundCoroutine);
            VictoryDialog();
        }else if (state.ToString() == "Playing")
        {
            stateIsPlaying = true;
            growingSoundCoroutine = StartCoroutine(GrowingSound());
        }
        else
        {
            if(growingSoundCoroutine != null)
            {
            StopCoroutine(growingSoundCoroutine);
            }
            stateIsPlaying = false;
        }
    }

    void Start()
    {
        scoreText.color = new Color(0.73f, 0.73f, 0.73f);
        // Get the Bloom component from the Volume
        postProcessingVolume.profile.TryGet<Bloom>(out bloomEffect);

        PreLevelDialog();
        //Invoke("PreLevelDialog", 1);
    }

    private void PreLevelDialog()
    {
        dialogController.lines = new string[3];
        dialogController.lines[0] = "We will test your Sensory Sentivity and Repetitive Behaviours";
        dialogController.lines[1] = "We understand that this may present challenges, but we have confidence in your abilities.";
        dialogController.lines[2] = "Remember, your objective remains the same: shoot and eliminate all the aliens before they overpower you. Stay focused and do your best.";
        GameManager.Instance.UpdateGameState(GameManager.GameState.Dialog);
    }

    private void VictoryDialog()
    {
        dialogController.lines = new string[4];
        dialogController.lines[0] = "Congratulations, human!";
        dialogController.lines[1] = "You have successfully completed the test that simulates what it might be like to experience autism.";
        dialogController.lines[2] = "Your understanding and adaptability are commendable.";
        dialogController.lines[3] = "Now, prepare yourself for the ultimate challenge";

    }

    // Update is called once per frame
    void Update()
    {
        if (stateIsPlaying)
        {
            if (Input.GetKeyDown(currentKeyCode))
            {
                if (sceneSound.volume > 0.13f)
                {
                    sceneSound.volume -= 0.1f;
                }
                if(bloomEffect.threshold.value < 1.5f)
                {
                    bloomEffect.threshold.value += 0.12f;
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
            growingSoundLoop++;
            if (bloomEffect.threshold.value > 0)
            {
                bloomEffect.threshold.value -= 0.05f;
            }
            sceneSound.volume += 0.05f;

            if(growingSoundLoop > 10){
                growingSoundLoop = 0;
                ChangeLetter();
            }
            if (stateIsPlaying)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void ChangeLetter(){
        fontIndex = Random.Range(0, fontsList.Count);
        TMP_FontAsset font = fontsList[fontIndex];
        textMeshPro.font = font;


        index = Random.Range(0, alphabet.Length);
        textMeshPro.text = "Press " + alphabet[index];
        currentKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), alphabet[index].ToString());
    }
}
