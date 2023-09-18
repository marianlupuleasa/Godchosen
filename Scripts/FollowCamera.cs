using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class FollowCamera : MonoBehaviour
{
    public bool canMove = true;
    void Update()
    {
        if(canMove && Time.timeScale != 0)
        {
            if(this.transform.position.y <= 40.5)
            {
                this.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, transform.position.z);
            }
            else if(this.transform.position.y <= 70.5)
            {
                this.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.3f, transform.position.z);
            }    
        }
        
    }
}
 
