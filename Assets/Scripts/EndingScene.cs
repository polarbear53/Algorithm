using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : ScenePlayer
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
        }
    }

    public override void StartCutscene()
    {
        isCutscenePlaying = true;
        redHood_wayIndex = 0;
        ZoomToTarget();

    }
}
