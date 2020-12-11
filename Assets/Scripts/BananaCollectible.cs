using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            //if (controller.health < controller.maxHealth)
            {
                controller.BananaPickUp();
                Destroy(gameObject);
            }
        }
    }
}