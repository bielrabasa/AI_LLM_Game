using System.Collections;
using UnityEngine;

public class RayScript : MonoBehaviour
{
    Transform platform;
    const float MAX_WIDTH_SCALE = 1f;

    void Start()
    {
        platform = GameObject.FindGameObjectWithTag("PLATFORM").transform;
        transform.position = new Vector3(platform.position.x, 18.5f, -0.5f);
        transform.localScale = new Vector3(0.0f, 30, 3);
        transform.parent = platform;
        gameObject.layer = 6;
        GetComponent<Collider>().enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(0.5f);

        float t = 0;
        while(transform.localScale.x < MAX_WIDTH_SCALE)
        {
            t += Time.deltaTime / 2;
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + t, 0f, MAX_WIDTH_SCALE), 
                transform.localScale.y, transform.localScale.z);
            
            yield return null;
        }

        GetComponent<Collider>().enabled = true;
        transform.parent = null;

        Destroy(gameObject, 0.5f);
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
