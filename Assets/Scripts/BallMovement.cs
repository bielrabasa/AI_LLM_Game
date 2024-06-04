using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour
{
    int carrying = 0;
    bool isExplosive = false;
    bool isChaotic = false;

    public float realInitialSpeed = 20f;
    float initialSpeed;
    public float speedIncrease = 0.2f;
    Rigidbody rb;

    public int lives = 5;

    Material mat;
    public Material powerMat;

    public GameObject explosion;

    Vector3 initialPos;

    //LLM
    LLM_Comunication llm;
    LayoutScript level;

    void Start()
    {
        llm = FindObjectOfType<LLM_Comunication>();
        level = FindObjectOfType<LayoutScript>();
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;

        initialPos = transform.position;
        ResetBall(true);
    }

    public void ResetBall(bool lvlChange)
    {
        //Reset Ball pos & direction
        transform.position = initialPos;
        rb.velocity = Vector3.zero;

        ResetAbilities();

        if (lvlChange)
        {
            initialSpeed = realInitialSpeed;
            llm.BossTalk(lives, level.levelCharged + 1);
            StartCoroutine(InitFirstShoot());
        }
        else
        {
            StartCoroutine(InitShoot());
        }
    }

    void ResetAbilities()
    {
        //ResetValues
        carrying = 0;
        isExplosive = false;
        isChaotic = false;
        GetComponent<Renderer>().material = mat;
    }

    IEnumerator InitShoot()
    {
        yield return new WaitForSeconds(2);

        Vector3 initialDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        rb.velocity = initialDirection * initialSpeed;
    }

    IEnumerator InitFirstShoot()
    {
        while (llm.isWriting) yield return null;

        yield return new WaitForSeconds(2);
        BossAbility();
    }

    public void IncreaseSpeed()
    {
        initialSpeed += speedIncrease;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LOSE"))
        {
            lives--;

            if (lives > 0) ResetBall(false);
            else level.Lose();

            return;
        }

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
            IncreaseSpeed();
        }
        else if (collision.gameObject.CompareTag("BLOCK_S"))
        {
            GetComponent<Renderer>().material = powerMat;

            carrying = Random.Range(1, 5);
            Destroy(collision.gameObject);
            
            IncreaseSpeed();
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

    //---------------- ABILITIES ----------------------

    private void CheckAbilities()
    {
        //In case of repeating ability, skip to instant ones
        if ((carrying == 1 && isChaotic) || (carrying == 3 && isExplosive)) carrying++;

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
                    ShootRay();
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
        AbilityMessage("Rearange!");
        FindObjectOfType<LayoutScript>().RearangeBricks();
    }

    void ShootRay()
    {
        AbilityMessage("You have rays?");
        GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<RayScript>();
    }

    IEnumerator ChaosMode()
    {
        AbilityMessage("Chaos!");

        isChaotic = true;
        yield return new WaitForSeconds(10);
        isChaotic = false;
        rb.velocity = rb.velocity.normalized * initialSpeed; //Return to normal vel
    }

    IEnumerator ExplosiveMode()
    {
        AbilityMessage("Explosive?");
        isExplosive = true;
        yield return new WaitForSeconds(10);
        isExplosive = false;
    }

    void AbilityMessage(string m)
    {
        llm.ui.text = m;
        StartCoroutine(EraseScreen());
    }

    IEnumerator EraseScreen()
    {
        yield return new WaitForSeconds(2);
        llm.ui.text = "";
    }

    public void BossAbility()
    {
       StartCoroutine(ReDirection());
    }

    IEnumerator ReDirection()
    {
        Debug.Log("Random Ball");
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(1);

        rb.velocity = new Vector2(Random.Range(-1f,1), Random.Range(0.1f, 1)).normalized
            * initialSpeed * Random.Range(2f, 3f);
    }
}
