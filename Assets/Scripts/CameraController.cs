using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
