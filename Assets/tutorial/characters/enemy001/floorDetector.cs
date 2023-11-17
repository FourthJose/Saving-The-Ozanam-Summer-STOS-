using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class enemyDetector : MonoBehaviour
{
    public static bool isOverGround;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Environment"))
        {
            isOverGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Environment"))
        {
            isOverGround = false;
        }
    }
}
