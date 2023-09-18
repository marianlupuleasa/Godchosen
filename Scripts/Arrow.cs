using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    [SerializeField] float arrowSpeed;
    Player player;
    float arrowXSpeed;
    [SerializeField] AudioClip arrowHitSFX;
    [SerializeField] AudioClip arrowBlockSFX;
    string sceneName;
    int gamePart;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        player = FindObjectOfType<Player>();
        arrowXSpeed = this.transform.localScale.x * arrowSpeed;
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if(sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3")
        {
            gamePart = 1;
        }
        else if(sceneName == "Level 4")
        {
            gamePart = 2;
        }
        
    }

    void Update()
    {
        myRigidbody2D.velocity = new Vector2(arrowXSpeed, 0f);

        if(gamePart == 2 && this.transform.position.x > 9)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if((other.tag == "Enemy" || other.tag == "Bow Enemy") && !other.GetComponent<EnemyMovement>().wasKilled)
        {
            AudioSource.PlayClipAtPoint(arrowHitSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().IncreaseScore(100);
            other.GetComponent<EnemyMovement>().wasKilled = true;
            Destroy(other.gameObject);

            if(gamePart == 2)
            {
                if(FindObjectOfType<GameSession>().gamePart == 2)
                {
                    FindObjectOfType<Boss>().BossSpawn();
                }
            }
        }

        if(other.tag == "Tough Enemy")
        {
            AudioSource.PlayClipAtPoint(arrowBlockSFX, Camera.main.transform.position);
        }

        if(other.tag == "Boss")
        {
            AudioSource.PlayClipAtPoint(arrowHitSFX, Camera.main.transform.position);
            FindObjectOfType<Boss>().TakeDamage();
        }

        if(other.tag == "Wall")
        {
            FindObjectOfType<TempleHealth>().Damage(5);
            AudioSource.PlayClipAtPoint(arrowHitSFX, Camera.main.transform.position);
        }

        Destroy(gameObject);

    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.rigidbody.tag == "Player")
        {
            if(FindObjectOfType<Player>().isPlayerAlive)
            {
                AudioSource.PlayClipAtPoint(arrowHitSFX, Camera.main.transform.position);
            }
            
            if(gamePart == 1)
            {
                FindObjectOfType<Player>().LoseLife();
            }
            else if(gamePart == 2)
            {
                FindObjectOfType<TempleHealth>().Damage(10);            
            }
            
        }

        if(other.rigidbody.tag == "Shield")
        {
            AudioSource.PlayClipAtPoint(arrowBlockSFX, Camera.main.transform.position);
        }

        Destroy(gameObject);

    }
}
