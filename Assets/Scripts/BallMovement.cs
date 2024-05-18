using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    delegate void AbilityActivation();
    List<AbilityActivation> ability = new List<AbilityActivation>();
    int carrying = 0;
    bool isExplosive = false;
    bool isChaotic = false;

    public float initialSpeed = 5f;
    Rigidbody rb;

    Material mat;
    public Material powerMat;

    void Start()
    {
        carrying = 0;
        isExplosive = false;
        isChaotic = false;
        ability.Clear();
        ability.Add(RearangeBricks);
        ability.Add(ShootRay);
        ability.Add(ChaosMode);
        ability.Add(ExplosiveMode);

        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;

        // Start the ball movement with a random direction
        Vector3 initialDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        rb.velocity = initialDirection * initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Platform
        if (collision.gameObject.CompareTag("PLATFORM"))
        {
            if (carrying != 0)
            {
                ability[carrying - 1]();
                carrying = 0;

                GetComponent<Renderer>().material = mat;
            }
        }

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

            carrying = Random.Range(1, 5);
            Destroy(collision.gameObject);
        }
    }

    void HandleExplosive()
    {
        //TODO: Create big ball that breaks bricks arround & doesn't collide with current ball
    }

    void HandleChaotic()
    {
        rb.velocity = rb.velocity.normalized * initialSpeed;
        rb.velocity *= Random.Range(0.5f, 2f);
    }

    void RearangeBricks()
    {
        Debug.Log("Rearange");
        FindObjectOfType<LayoutScript>().RearangeBricks();
    }
    void ShootRay()
    {
        Debug.Log("Shoot");
        StartCoroutine(Ray());
    }

    IEnumerator Ray()
    {
        yield return new WaitForSeconds(0.1f);
        //TODO: Shoot
    }

    void ChaosMode()
    {
        Debug.Log("Chaos");
        StartCoroutine(Chaotic());
    }

    IEnumerator Chaotic()
    {
        isChaotic = true;

        yield return new WaitForSeconds(10);

        isChaotic = false;
        rb.velocity = rb.velocity.normalized * initialSpeed; //Return to normal vel
    }

    void ExplosiveMode()
    {
        Debug.Log("Explosive");
        StartCoroutine(Explosive());
    }

    IEnumerator Explosive()
    {
        isExplosive = true;
        yield return new WaitForSeconds(10);
        isExplosive = false;
    }
}
