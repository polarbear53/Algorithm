using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : ScenePlayer
{
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = mainCamera.transform.position;
        originalZoom = mainCamera.orthographicSize;
        StartCutscene();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCutscenePlaying)
        {
            RedHoodMove();

            WolfMove();
        }
    }
}
