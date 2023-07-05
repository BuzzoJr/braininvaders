using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherMissileController : MonoBehaviour
{
    private bool stateIsPlaying = true;
    public float speed = 5f; // Speed of the missile
    private float duration = 8f;

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
            DestroyObject();
        }
        else if (state.ToString() == "Playing")
        {
            stateIsPlaying = true;
        }
        else
        {
            stateIsPlaying = false;
        }
    }

    void Update()
    {
        // Move the missile downwards
        if (stateIsPlaying)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        Invoke("DestroyObject", duration);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            DestroyObject();
        }
    }
}