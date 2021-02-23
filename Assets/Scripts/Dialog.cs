using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    int playerLayer;
    public GameObject dialog;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==playerLayer&&collision)
        {
            dialog.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        dialog.SetActive(false);
        
    }
}
