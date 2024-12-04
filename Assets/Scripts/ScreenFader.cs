using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{

    public Image fadeImage;
    public float fadeDuration = 1.0f;

    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color color = fadeImage.color;
        float time = 0;

        while (time < fadeDuration)
        {
            color.a = Mathf.Clamp01(time / fadeDuration); // Alpha 값 증가
            fadeImage.color = color;

            time += Time.deltaTime;

            Debug.Log($"Fade Alpha: {color.a}, Time: {time}");

            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        LoadNextscene();
        
    }
    protected void LoadNextscene()
    {
        Debug.Log("다음 씬 로드");
        SceneManager.LoadScene(nextScene);
    }
}
