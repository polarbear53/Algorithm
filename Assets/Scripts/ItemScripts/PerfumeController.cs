using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfumeController : ItemController
{
    protected override void ItemGet()
    {
        WolfController obj = GameObject.Find("Wolf").GetComponent<WolfController>();
        speed = -0.33f;
        durTime = 3.0f;
        obj.StartCoroutine(obj.Slow(speed, durTime));
    }
}
