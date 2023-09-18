using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody2D;
    public Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    SpriteRenderer mySpriteRenderer;
    GameObject groundTag;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowTransform;
    [SerializeField] float arrowAnimation;
    [SerializeField] GameObject sword;
    [SerializeField] Transform swordTransform;
    [SerializeField] float swordAnimation;
    [SerializeField] GameObject shield;
    [SerializeField] Transform shieldTransform;
    [SerializeField] float shieldAnimation;
    bool canAttack = true;
    bool canMove = true;
    bool isSwordEquipped;
    bool isShieldEquipped;

    [Header("Speed")]
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;


    float normalGravity;
    public bool isPlayerAlive = true;
    bool canPlayerTakeDamage = true;
    public bool isPlayerStunned = false;
    Vector2 deathVelocity = new Vector2 (5, 5);
    Color deathColor = Color.red;
    string sceneName;
    int gamePart;
    float verticalDirection = 0;
    float horizontalDirection = 0;

    [SerializeField] AudioClip swordAttackSFX;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        normalGravity = myRigidbody2D.gravityScale;
        groundTag = GameObject.FindGameObjectWithTag("Ground");
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
        else if(sceneName == "Level 5")
        {
            gamePart = 3;
        }
        
        
    }

    void Update()
    {
        if(!isPlayerAlive)
        {
            return;
        }

        if(!PauseMenu.isPaused && !isPlayerStunned)
        {
            if(gamePart == 1)
            {
                Climb();
                Run();

                if(myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Trap")))
                {
                    Restart();
                }
                if(myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
                {
                    LoseLife();
                }
            }

            if(gamePart == 2)
            {
                if(Input.GetKeyDown(KeyCode.W))
                {
                    verticalDirection = 1;
                } 
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    verticalDirection = -1;
                }
                if(canMove)
                {
                    VerticalFly(); 
                }
            }
            
            if(gamePart == 3)
            {
                if(Input.GetKeyDown(KeyCode.D))
                {
                    horizontalDirection = 1;
                }
                else if(Input.GetKeyDown(KeyCode.A))
                {
                    horizontalDirection = -1;
                }
                if(canMove)
                {
                    HorizontalFly(); 
                }
                if(FindObjectOfType<FollowCamera>().transform.position.y - transform.position.y > 3)
                {
                    transform.position = new Vector2 (-1.5f, FindObjectOfType<FollowCamera>().transform.position.y - 0.99f);
                    FindObjectOfType<PlayerHealth>().Damage(10);
                }
                else if(transform.position.y < 67.5f)
                {
                    transform.position = new Vector2 (transform.position.x, transform.position.y + 0.1f);
                }
                else
                {
                    transform.position = new Vector2 (transform.position.x, transform.position.y + 0.3f);
                }
            }
        }
        
    }

    void OnMove(InputValue value)
    {
        if(!isPlayerAlive)
        {
            return;
        }

        if(!PauseMenu.isPaused && gamePart == 1)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    { 
        if(!isPlayerAlive)
        {
            return;
        }

        if(gamePart == 1)
        {
            if(!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                return;
            }

            if(!PauseMenu.isPaused)
            {
                if(value.isPressed)
                {
                    myRigidbody2D.velocity += new Vector2 (0, jumpSpeed);
                }
            }      
        }
    }

    void OnSwordAttack(InputValue value)
    {
        if(!isPlayerAlive)
        {
            return;
        }

        if(value.isPressed && canAttack && !PauseMenu.isPaused && !isPlayerStunned && gamePart != 3)
        {   
            GameObject swordInstance = Instantiate(sword, swordTransform.position, transform.rotation, transform);
            swordInstance.transform.localScale = new Vector3(2 * transform.localScale.x, swordInstance.transform.localScale.y,swordInstance.transform.localScale.z);
                
            canAttack = false;
            isSwordEquipped = true;
            myAnimator.SetBool("IsSwordAttacking", true);
            Invoke("StartAttacking",swordAnimation);
            AudioSource.PlayClipAtPoint(swordAttackSFX, Camera.main.transform.position);
            Destroy(swordInstance,swordAnimation);
        }
    }

    void OnBowAttack(InputValue value)
    {
        if(!isPlayerAlive)
        {
            return;
        }

        if(value.isPressed && canAttack && !PauseMenu.isPaused && !isPlayerStunned && gamePart != 3)
        {   
            myAnimator.SetBool("IsBowAttacking", true);
            canAttack = false;
            Invoke("ArrowAttack",0.5f);
        }
    }

    void ArrowAttack()
    {
        GameObject arrowInstance = Instantiate(arrow, arrowTransform.position, transform.rotation);
        arrowInstance.transform.localScale = new Vector3(transform.localScale.x, arrowInstance.transform.localScale.y,arrowInstance.transform.localScale.z);

        myAnimator.SetBool("IsBowAttacking", false);
        canAttack = true;
    }

    void OnShieldParry(InputValue value)
    {
        if(!isPlayerAlive)
        {
            return;
        }

        if(value.isPressed && canAttack && !PauseMenu.isPaused && !isPlayerStunned && gamePart != 3)
        {   
            GameObject shieldInstance = Instantiate(shield, shieldTransform.position, transform.rotation, transform);
            shieldInstance.transform.localScale = new Vector3(transform.localScale.x, shieldInstance.transform.localScale.y,shieldInstance.transform.localScale.z);

            canAttack = false;
            isShieldEquipped = true;
            myAnimator.SetBool("IsShieldParrying", true);
            Invoke("StartAttacking",shieldAnimation);
            Destroy(shieldInstance,shieldAnimation);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        bool isMovingHorizontally = myRigidbody2D.velocity.x != 0;
        myAnimator.SetBool("IsRunning", isMovingHorizontally);

        if(isMovingHorizontally)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody2D.velocity.x), 1);
        }    
    }
    void VerticalFly()
    {
        if(!(transform.position.y == 3.5 && verticalDirection == 1) && !(transform.position.y == -4.5 && verticalDirection == -1))
        {
            this.transform.position = new Vector2 (transform.position.x, transform.position.y + 2 * verticalDirection);
            verticalDirection = 0;

            bool isMovingVertically = myRigidbody2D.velocity.y != 0;

            canMove = false;
            Invoke("StartMoving",0.1f);
        }
    }

    void HorizontalFly()
    {
        if(this.transform.position.y <= 37.5)
        {
            if(!(transform.position.x >= 0.5 && horizontalDirection == 1) && !(transform.position.x <= -3.5 && horizontalDirection == -1))
            {
                this.transform.position = new Vector2 (transform.position.x + horizontalDirection, transform.position.y);
                horizontalDirection = 0;

                bool isMovingHorizontally = myRigidbody2D.velocity.y != 0;

                canMove = false;
                Invoke("StartMoving",0.1f);
            }
        }
        else if(this.transform.position.y <= 67.5)
        {
            if(!(transform.position.x >= -0.5 && horizontalDirection == 1) && !(transform.position.x <= -2.5 && horizontalDirection == -1))
            {
                this.transform.position = new Vector2 (transform.position.x + horizontalDirection, transform.position.y);
                horizontalDirection = 0;

                bool isMovingHorizontally = myRigidbody2D.velocity.y != 0;

                canMove = false;
                Invoke("StartMoving",0.2f);
            }
        }
        else
        {
            canMove = false;
        }
    }

    void Climb()
    {
         if(!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("IsClimbing", false);
            myRigidbody2D.gravityScale = normalGravity;
            return;
        }

        if(!PauseMenu.isPaused)
        {
            Vector2 playerVelocity = new Vector2 (myRigidbody2D.velocity.x, moveInput.y * climbSpeed);
            myRigidbody2D.velocity = playerVelocity;
            myRigidbody2D.gravityScale = 0;

            bool isMovingVertically = myRigidbody2D.velocity.y != 0;
            myAnimator.SetBool("IsClimbing", isMovingVertically);
        }
        

    }

    public void Restart()
    {
        if(isPlayerAlive && !PauseMenu.isPaused)
        {
            isPlayerAlive = false;
            myAnimator.SetBool("IsRunning", false);
            myAnimator.SetTrigger("Dead");
            mySpriteRenderer.color = deathColor;
            myRigidbody2D.velocity += deathVelocity;
            FindObjectOfType<GameSession>().ProcessPlayerDeathOnTrap();
        }
        
    }

    public void LoseLife()
    {
        if(canPlayerTakeDamage && !PauseMenu.isPaused)
        {
            canPlayerTakeDamage = false;
            FindObjectOfType<GameSession>().ProcessPlayerDeathOnEnemy();
            Invoke("CanTakeDamage",1f);
        }
        
    }

    void StartAttacking()
    {
        if(isSwordEquipped)
        {
            myAnimator.SetBool("IsSwordAttacking", false);
            canAttack = true;
            isSwordEquipped = false;
        }
        else if(isShieldEquipped)
        {
            myAnimator.SetBool("IsShieldParrying", false);
            canAttack = true;
            isShieldEquipped = false;
        }
    }

    void StartMoving()
    {
        canMove = true;
    }

    void CanTakeDamage()
    {
        canPlayerTakeDamage = true;
    }
}
