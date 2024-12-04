using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : ItemController
{
    protected override void ItemGet()
    {
        if (player.Cloak != 0)
        {
            Debug.Log("¸ÁÅä °³¼ö: " + --player.Cloak);
            return;
        }
        speed = -0.33f;
        durTime = 3.0f;
        player.StartCoroutine(player.Slow(speed, durTime));
    }
}
