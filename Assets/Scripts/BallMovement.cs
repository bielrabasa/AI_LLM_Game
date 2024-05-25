using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    int carrying = 0;
    bool isExplosive = false;
    bool isChaotic = false;

    public float initialSpeed = 5f;
    Rigidbody rb;

    Material mat;
    public Material powerMat;

    public GameObject explosion;

    void Start()
    {
        carrying = 0;
        isExplosive = false;
        isChaotic = false;

        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;

        // Start the ball movement with a random direction
        //TODO: Uncomment
        //Vector3 initialDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        Vector3 initialDirection = new Vector2(0f, -1f).normalized;

        rb.velocity = initialDirection * initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = rb.velocity.normalized * initialSpeed;

        //Platform
        if (collision.gameObject.CompareTag("PLATFORM"))
        {
            CheckPlatformCollision(collision);
            return;
        }

        //Check for abilities on collision
        if(isExplosive) HandleExplosive();
        if(isChaotic) HandleChaotic();

        //Break Blocks
        if (collision.gameObject.CompareTag("BLOCK"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("BLOCK_S"))
        {
            GetComponent<Renderer>().material = powerMat;

            carrying = 3;//Random.Range(1, 5);  //TODO: UNcomment
            Destroy(collision.gameObject);
        }
    }

    private void CheckPlatformCollision(Collision collision)
    {
        CheckAbilities();

        //Redirect ball
        float contactPos = (transform.position.x - collision.transform.position.x) / 3f; // btw(-1, 1)

        float angle = 80f * contactPos * Mathf.Deg2Rad;
        Vector3 departure = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

        rb.velocity = departure.normalized * initialSpeed;
    }

    private void CheckAbilities()
    {
        //In case of repeating ability, skip to instant ones
        if (carrying == 1 && isChaotic) carrying++;
        if (carrying == 3 && isExplosive) carrying++;

        switch (carrying)
        {
            case 1:
                {
                    StartCoroutine(ChaosMode());
                    break;
                }
            case 2:
                {
                    RearangeBricks(); 
                    break;
                }
            case 3:
                {
                    StartCoroutine(ExplosiveMode());
                    break;
                }
            case 4:
                {
                    StartCoroutine(ShootRay());
                    break;
                }
            default:
                return;
        }

        carrying = 0;
        GetComponent<Renderer>().material = mat;
    }

    //---------------- HANDLE ABILITIES ----------------------

    void HandleExplosive()
    {
        //TODO: Make explosion particles
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    void HandleChaotic()
    {
        rb.velocity *= Random.Range(0.5f, 2f);
    }


    //---------------- ACTIVATE ABILITIES ----------------------

    void RearangeBricks()
    {
        //Debug.Log("Rearange");
        FindObjectOfType<LayoutScript>().RearangeBricks();
    }

    IEnumerator ShootRay()
    {
        yield return new WaitForSeconds(0.1f);
        //TODO: Shoot
    }

    IEnumerator ChaosMode()
    {
        //TODO: Indicate ability
        isChaotic = true;
        yield return new WaitForSeconds(10);
        isChaotic = false;
        rb.velocity = rb.velocity.normalized * initialSpeed; //Return to normal vel
    }

    IEnumerator ExplosiveMode()
    {
        //TODO: Indicate ability
        isExplosive = true;
        yield return new WaitForSeconds(10);
        isExplosive = false;
    }
}
