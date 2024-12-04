using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StartLightManager : MonoBehaviour
{
    Color dayColor = Color.gray;
    Color nightColor = new Color(12 / 255f, 28 / 255f, 60 / 255f);
    public Light2D globalLight;
    float time;
    // Start is called before the first frame update
    void Awake()
    {
        globalLight.color = dayColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        globalLight.color = Color.Lerp(dayColor, nightColor, time/5);
    }
}
