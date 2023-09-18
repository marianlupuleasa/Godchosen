using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    Animator myAnimator;
    public int enemiesDefeated = 0;
    public int enemiesToDefeat = 25;
    [SerializeField] float fireAnimLength;
    public bool canFire = true;
    int canFireSpecial = 0;
    public bool canMove = true;
    bool bossSpawned = false;
    bool isBossStunned = false;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowTransform;
    [SerializeField] GameObject shockwave;
    [SerializeField] Transform shockwaveTransform;
    float bossHealth = 150f;
    float[] bossPositions = { 3.1f, 1.1f, -0.9f, -2.9f, -4.9f};
    int healthLeft;
    [SerializeField] AudioClip arrowLaunchSFX;
    [SerializeField] AudioClip screamSFX;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(bossSpawned && !isBossStunned)
        {
                if(canFireSpecial == 5)
                {
                    if(canFire)
                    {
                        transform.position = new Vector2 (transform.position.x, -0.9f);
                        myAnimator.SetBool("IsSpecialAttacking", true);
                        Invoke("FireSpecial",1f);
                        Invoke("FireSpecial",1.1f);
                        Invoke("FireSpecial",1.2f);
                        canFire = false;
                        canMove = false;
                        Invoke("StartMoving",3f);
                        Invoke("StartAttacking",3f);
                    }
                    
                }
                else if(canMove && canFire)
                {
                    Move();
                    myAnimator.SetBool("IsAttacking", true);
                    Invoke("Fire",1f);
                    canFire = false;
                }
            
        }
        
    }
    
    public void BossSpawn()
    {
        enemiesDefeated++;
        if(enemiesDefeated == enemiesToDefeat)
        {
            this.transform.position = new Vector2 (transform.position.x - 5, transform.position.y);
            bossSpawned = true;
            FindObjectOfType<GameSession>().bossBar.SetActive(true);
            canMove = false;
            Invoke("StartMoving",1f);
        }
    }

    void Fire()
    {
        GameObject arrowInstance = Instantiate(arrow, arrowTransform.position, transform.rotation);
        arrowInstance.transform.localScale = new Vector3(transform.localScale.x, arrowInstance.transform.localScale.y, arrowInstance.transform.localScale.z);

        canFireSpecial++;
        AudioSource.PlayClipAtPoint(arrowLaunchSFX, Camera.main.transform.position);
        myAnimator.SetBool("IsAttacking", false);
        Invoke("StartAttacking",1f);
    }

    void FireSpecial()
    {
        GameObject shockwaveInstance = Instantiate(shockwave, shockwaveTransform.position, transform.rotation);
        shockwaveInstance.transform.localScale = new Vector3(transform.localScale.x, shockwaveInstance.transform.localScale.y, shockwaveInstance.transform.localScale.z);

        canFireSpecial = 0;
        AudioSource.PlayClipAtPoint(screamSFX, Camera.main.transform.position);

        myAnimator.SetBool("IsSpecialAttacking", false);
    }

    void Move()
    {
        float position = bossPositions[Random.Range(0, bossPositions.Length)];
        transform.position = new Vector2 (transform.position.x, position);

        canMove = false;
        Invoke("StartMoving",1f);
    }

    void StartAttacking()
    {
        myAnimator.SetBool("IsAttacking", false);
        canFire = true;
    }

    void StartMoving()
    {
        canMove = true;
    }

    public void TakeDamage()
    {
        bossHealth -= 5f;
        FindObjectOfType<GameSession>().bossHealth.fillAmount = bossHealth / 150f;

        if(bossHealth <= 0f)
        {
            Destroy(gameObject);
            FindObjectOfType<GameSession>().IncreaseScore(1000);
            healthLeft = FindObjectOfType<TempleHealth>().healthAmount;
            FindObjectOfType<GameSession>().IncreaseScore(20 * healthLeft);

            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene("Level 5");
        }
    }

    public void StunPlayer()
    {
        FindObjectOfType<Player>().isPlayerStunned = true;
        FindObjectOfType<Player>().myAnimator.SetBool("IsPetrified", true);
        Invoke("UnstunPlayer",5f);
    }

    void UnstunPlayer()
    {
        FindObjectOfType<Player>().isPlayerStunned = false;
        FindObjectOfType<Player>().myAnimator.SetBool("IsPetrified", false);
    }

    public void StunBoss()
    {
        isBossStunned = true;
        if(FindObjectOfType<Player>().isPlayerStunned)
        {
            UnstunBoss();
        }
        else
        {
            Invoke("UnstunBoss",5f);
        } 
    }

    void UnstunBoss()
    {
        isBossStunned = false;
    }
}
