using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherController : ItemController
{
    protected override void ItemGet()
    {
        speed = 0.5f;
        durTime = 3.0f;
        player.StartCoroutine(player.Slow(speed, durTime));
    }
}
