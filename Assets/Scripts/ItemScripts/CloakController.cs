using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakController : ItemController
{
    protected override void ItemGet()
    {
        Debug.Log("���� ����: " + ++player.Cloak);
    }
}
