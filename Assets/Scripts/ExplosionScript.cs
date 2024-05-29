using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.1f);
        GetComponent<ParticleSystem>().Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Break Blocks
        if (collision.gameObject.CompareTag("BLOCK") || 
            collision.gameObject.CompareTag("BLOCK_S") || 
            collision.gameObject.CompareTag("BLOCK_U"))
        {
            Destroy(collision.gameObject);
            FindObjectOfType<BallMovement>().IncreaseSpeed();
        }
    }
}
