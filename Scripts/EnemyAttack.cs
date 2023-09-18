using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float fireInterval;
    bool canFire = true;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowTransform;
    Animator myAnimator;
    string sceneName;
    bool stopMoving = false;

    void Start()
    {
        myAnimator = GetComponent<Animator>();

        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if(sceneName == "Level 4")
        {
            canFire = false;
        }
        
    }
    void Update()
    {
        
        if(sceneName == "Level 4" && this.tag == "Bow Enemy" && transform.position.x <= 8 && !stopMoving)
        {
            canFire = true;
            stopMoving = true;
        }

        if(canFire)
        {
            myAnimator.SetBool("IsAttacking", true);
            Invoke("Fire",2);
            canFire = false;

        }
        
    }

    void Fire()
    {
        GameObject arrowInstance = Instantiate(arrow, arrowTransform.position, transform.rotation);
        arrowInstance.transform.localScale = new Vector3(transform.localScale.x, arrowInstance.transform.localScale.y, arrowInstance.transform.localScale.z);

        canFire = true;
        myAnimator.SetBool("IsAttacking", false);
    }
}
