using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sword : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    [SerializeField] AudioClip killSFX;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();

    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if((other.tag == "Enemy" || other.tag == "Tough Enemy" || other.tag == "Bow Enemy") && !other.GetComponent<EnemyMovement>().wasKilled)
        {
            AudioSource.PlayClipAtPoint(killSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().IncreaseScore(100);
            other.GetComponent<EnemyMovement>().wasKilled = true;
            Destroy(other.gameObject);

            if(FindObjectOfType<GameSession>().gamePart == 2)
            {
                FindObjectOfType<Boss>().BossSpawn();
            }
        }

        Destroy(gameObject);
    }
}
