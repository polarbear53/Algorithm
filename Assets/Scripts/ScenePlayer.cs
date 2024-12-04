using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ScenePlayer : MonoBehaviour
{
    public Transform redHood;
    public Transform wolf;

    public Vector3[] redHood_waypoints; // 빨간모자 이동 경로 좌표
    public Vector3[] wolf_waypoints; // 늑대 이동 경로 좌표
    protected int redHood_wayIndex = 0;
    protected int wolf_wayIndex = 0;


    public float speed = 1.0f; // 이동 속도

    protected bool isCutscenePlaying = false;

    public Camera mainCamera;
    public Vector3 targetPosition;
    public float targetZoom = 5f;
    public float duration = 1.0f;

    protected Vector3 originalPosition;
    protected float originalZoom;

    public ScreenFader screenFader;

    public virtual void StartCutscene()
    {
        isCutscenePlaying = true;
        redHood_wayIndex = 0;
        wolf_wayIndex = 0;

    }

    private void SetAnimation(Vector3 targetPosition, Transform character)
    {
        Vector3 direction = targetPosition - character.position;

        Animator animator = character.GetComponent<Animator>();
        SpriteRenderer spriter = character.GetComponent<SpriteRenderer>();

        animator.SetFloat("y", direction.y);

        if (direction.x != 0)
        {
            animator.SetBool("x", true);
            spriter.flipX = direction.x < 0;
        }
        else animator.SetBool("x", false);
    }

    protected virtual void RedHoodMove()
    {
        if (redHood_wayIndex >= redHood_waypoints.Length)
        {
            //EndCutscene();
            return;
        }

        Vector3 targetPosition = redHood_waypoints[redHood_wayIndex];
        redHood.position = Vector3.MoveTowards(redHood.position, targetPosition, speed * Time.deltaTime);

        SetAnimation(targetPosition, redHood);

        // 목표 지점에 도달했는지 확인
        if (Vector3.Distance(redHood.position, targetPosition) == 0)
        {
            redHood_wayIndex++;
        }
    }

    protected virtual void WolfMove()
    {
        if (wolf_wayIndex >= wolf_waypoints.Length)
        {
            //EndCutscene();
            return;
        }

        Vector3 targetPosition = wolf_waypoints[wolf_wayIndex];
        wolf.position = Vector3.MoveTowards(wolf.position, targetPosition, 0.8f * Time.deltaTime);

        SetAnimation(targetPosition, wolf);

        // 목표 지점에 도달했는지 확인
        if (Vector3.Distance(wolf.position, targetPosition) == 0)
        {
            ZoomToTarget();
            wolf_wayIndex++;
        }
    }

    public void ZoomToTarget()
    {
        StartCoroutine(ZoomCoroutine());
    }

    protected virtual IEnumerator ZoomCoroutine()
    {
        float time = 0;
        Vector3 startPosition = mainCamera.transform.position;
        float startZoom = mainCamera.orthographicSize;

        while (time < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetZoom;

        screenFader.StartFadeOut();

        EndCutscene();
    }

    protected void EndCutscene()
    {
        isCutscenePlaying = false;

        Debug.Log("컷신 종료");
    }

}
