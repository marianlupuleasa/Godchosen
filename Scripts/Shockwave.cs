using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shockwave : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    [SerializeField] float shockwaveSpeed;
    Player player;
    float shockwaveXSpeed;
    int scaleShockwave = 0;
    [SerializeField] AudioClip killSFX;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        shockwaveXSpeed = this.transform.localScale.x * shockwaveSpeed;
        
    }

    void Update()
    {
        myRigidbody2D.velocity = new Vector2(shockwaveXSpeed, 0f);
        if(scaleShockwave < 500)
        {
            this.transform.localScale = new Vector2(this.transform.localScale.x * 1.015f, this.transform.localScale.y * 1.03f);
            scaleShockwave++;
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.rigidbody.tag == "Player")
        {
            FindObjectOfType<Boss>().StunPlayer();  
        }

        if(other.rigidbody.tag == "Shield")
        {
            FindObjectOfType<Boss>().StunBoss();
        }

        Destroy(gameObject);

    }
}
