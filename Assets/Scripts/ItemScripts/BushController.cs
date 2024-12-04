using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushController : ItemController
{
    protected override void ItemGet()
    {
        if (player.Cloak != 0)
        {
            Debug.Log("¸ÁÅä °³¼ö: " + --player.Cloak);
            return;
        }
        speed = 0;
        durTime = 0;
        player.StartCoroutine(player.Slow(speed, durTime, 3));
    }
}
