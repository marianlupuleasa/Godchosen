using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickUp : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSFX;
    bool wasPickedUp = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !wasPickedUp)
        {
            AudioSource.PlayClipAtPoint(pickUpSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().HealPlayer();
            Destroy(gameObject);
            wasPickedUp = true;
        }
    }
}
