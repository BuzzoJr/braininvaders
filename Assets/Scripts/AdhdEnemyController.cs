using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdhdEnemyController : MonoBehaviour
{
    public static System.Action<AdhdEnemyController> hitEnemy;
    private AudioSource scoreAudio;
    private List<int> xPosition;
    // Start is called before the first frame update
    void Awake()
    {
        xPosition = new List<int> { -6, 0, 6 };
        scoreAudio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            int randomIndex = Random.Range(0, xPosition.Count);
            int randomXPos = xPosition[randomIndex];
            transform.position = new Vector3(randomXPos, transform.position.y, transform.position.z);
            AdhdEnemyController.hitEnemy(this);
            scoreAudio.Play();
        }
    }
}
