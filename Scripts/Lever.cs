using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] GameObject fakeWall;
    Animator myAnimator;
    bool wasPulled = false;
    [SerializeField] float leverPullAnimationDuration;

    void Start(){
        
        myAnimator = GetComponent<Animator>();

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !wasPulled)
        {
            myAnimator.SetBool("IsBeingPulled", true);
            wasPulled = true;
            Destroy(fakeWall, leverPullAnimationDuration);

        }
    }
}
