using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayoutScript : MonoBehaviour
{
    public Material bmat;
    public Material umat;
    public string[] lines = new string[8];
    int uBlocks = 0;

    void Start()
    {
        uBlocks = 0;

        Vector3 pos = new Vector3(-15, 30, 0);
        foreach(string line in lines)
        {
            foreach(char c in line)
            {
                if (c == '.') pos.x += 1.5f;
                else if (c == '-') pos.x += 3f;
                else if("BSU".Contains(c)){
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = pos;
                    cube.transform.localScale = new Vector3(2.5f, 1.2f, 1);
                    cube.transform.parent = transform;
                    pos.x += 3f;
                    cube.GetComponent<Renderer>().material = bmat;

                    if (c == 'B') cube.tag = "BLOCK";
                    else if (c == 'S') cube.tag = "BLOCK_S";
                    else if (c == 'U')
                    {
                        cube.tag = "BLOCK_U";
                        cube.GetComponent<Renderer>().material = umat;
                        uBlocks++;
                    }
                }
            }
            pos.x = -15;
            pos.y -= 2;
        }
    }

    private void Update()
    {
        if(transform.childCount <= uBlocks)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).CompareTag("BLOCK")) return;
            }

            Debug.Log("Win");
            SceneManager.LoadScene("MainScene");
        }
    }

    public void RearangeBricks()
    {
        int vertical = Random.Range(0, 4);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if(vertical == 0 && vertical == 1)
            {
                float y = child.position.y;
                y += 2 * (23 - y); //mirror from horizontal center line
                child.position = new Vector3(child.position.x, y, 0);
            }
            else if(vertical == 2)
            {
                float x = child.position.x;
                x *= -1; //mirror from center
                child.position = new Vector3(x, child.position.y, 0);
            }
            else
            {
                float y = child.position.y;
                float x = child.position.x;
                y += 2 * (23 - y); //mirror from horizontal center line
                x *= -1; //mirror from center
                child.position = new Vector3(x, y, 0);
            }
        }
    }
}
