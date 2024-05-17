using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public float speed = 10f;        // Speed of the paddle
    public float maxX = 8f;          // Horizontal bounds

    private float input;             // Input value for movement

    void Update()
    {
        // Get horizontal input (left/right arrow keys or A/D keys)
        input = Input.GetAxis("Horizontal");

        // Calculate the new position
        Vector3 newPosition = transform.position + Vector3.right * input * speed * Time.deltaTime;

        // Clamp the position within the bounds
        newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);

        // Assign the new position to the paddle
        transform.position = newPosition;
    }
}
