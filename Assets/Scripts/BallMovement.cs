using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float initialSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Start the ball movement with a random direction
        Vector3 initialDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        rb.velocity = initialDirection * initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BLOCK"))
        {
            Destroy(collision.gameObject);
        }
        /*// Reflect the ball's velocity based on the collision normal
        Debug.Log(rb.velocity);
        Vector3 reflectDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
        rb.velocity = reflectDirection * initialSpeed;*/
    }
}
