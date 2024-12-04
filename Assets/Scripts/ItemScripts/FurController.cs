using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurController : ItemController
{
    protected override void ItemGet()
    {
        WolfController obj = GameObject.Find("Wolf").GetComponent<WolfController>();
        if (player.Cloak != 0)
        {
            Debug.Log("¸ÁÅä °³¼ö: " + --player.Cloak);
            return;
        }
        speed = 0.33f;
        durTime = 3.0f;
        obj.StartCoroutine(obj.Slow(speed, durTime));
    }
}
