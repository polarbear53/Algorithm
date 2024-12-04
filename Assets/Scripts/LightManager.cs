using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    Color dayColor = Color.white;
    Color nightColor = new Color(0 / 255f, 1 / 255f, 3 / 255f);
    public Light2D globalLight;
    float time;
    // Start is called before the first frame update
    void Awake()
    {
        globalLight.color = nightColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = GameManager.Instance.time;
        float duringNight = 90;
        if (time > duringNight)
        {
            globalLight.color = Color.Lerp(nightColor, dayColor, (time - duringNight) / 90f);
        }
    }
}
