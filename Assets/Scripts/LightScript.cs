using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public float speed = 1;
    float currentH = 0;
    Light light;

    void Start()
    {
        currentH = 0;
        light = GetComponent<Light>();
    }

    void Update()
    {
        currentH += Time.deltaTime * speed;
        if (currentH > 360f) currentH -= 360;

        light.color = Color.HSVToRGB(currentH / 360f, 0.25f, 1);
    }
}
