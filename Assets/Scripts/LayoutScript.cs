using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutScript : MonoBehaviour
{
    public string[] lines = new string[8];

    void Start()
    {

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
                    cube.transform.localScale = new Vector3(2, 1, 1);
                    cube.transform.parent = transform;
                    pos.x += 3f;

                    if (c == 'B') cube.tag = "BLOCK";
                    else if (c == 'S') cube.tag = "BLOCK_S";
                    else if (c == 'U') cube.tag = "BLOCK_U";
                }
            }
            pos.x = -15;
            pos.y -= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
