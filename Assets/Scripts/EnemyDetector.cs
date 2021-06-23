using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            transform.parent.GetComponent<Enemy>().LaserFound(true);
            Debug.Log("Laser detected");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            transform.parent.GetComponent<Enemy>().LaserFound(false);
        }
    }
}
