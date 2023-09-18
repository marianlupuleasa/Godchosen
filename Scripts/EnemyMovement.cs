using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    BoxCollider2D myFaceCollider2D;
    CapsuleCollider2D myBodyCollider2D;
    public float runSpeed = 2;
    float enemyDirection;
    string sceneName;
    int gamePart;
    public bool wasKilled = false;
    [SerializeField] AudioClip swordHitSFX;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myFaceCollider2D = GetComponent<BoxCollider2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
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
        if(this.transform.localScale.x == 1)
        {
           myRigidbody2D.velocity = new Vector2 (runSpeed, 0); 
        }
        else
        {   

            myRigidbody2D.velocity = new Vector2 (-runSpeed, 0);
        }

        if(gamePart == 2 && this.tag == "Bow Enemy" && transform.position.x <= 8)
        {
            runSpeed = 0;
        }

        if(gamePart == 2 && this.tag == "Tough Enemy" && transform.position.x <= -7.5)
        {
            runSpeed = 0;
        } 
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            if(gamePart == 1)
            {
                transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody2D.velocity.x)), 1);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if((other.tag == "Wall" || other.tag == "Player") && this.tag != "Tough Enemy" && gamePart == 2 && !wasKilled)
        {
            FindObjectOfType<TempleHealth>().Damage(5);
            AudioSource.PlayClipAtPoint(swordHitSFX, Camera.main.transform.position);

            if(FindObjectOfType<GameSession>().gamePart == 2)
            {
                FindObjectOfType<Boss>().BossSpawn();
            }

            Destroy(gameObject);
            wasKilled = true;
        }
    }
}
