using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Tips : MonoBehaviour
{
    [SerializeField] GameObject tipText;
    // Start is called before the first frame update
    void Start()
    {
        tipText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.tag == "Player")
        {
            tipText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.tag == "Player")
        {
            tipText.SetActive(false);
        }
    }

}
