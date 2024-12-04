using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourScript : MonoBehaviour
{
    public float speed;
    public float buff;
    public int stop;
    public int Cloak;
    public int maxSpacebar = 10;
    public int spacebar;
    public GameObject sbContainer;

    public IEnumerator Slow(float speed, float durTime, int rev = 0)
    {
        float elapsedtime = 0.0f;
        switch (rev) 
        { 
            case 0:
                buff += speed;
                while (elapsedtime < durTime)
                {
                    elapsedtime += Time.deltaTime;
                    yield return null;
                }
                buff -= speed;
                break;
            case 1:
                this.speed *= -1;
                while (elapsedtime < durTime)
                {
                    elapsedtime += Time.deltaTime;
                    yield return null;
                }
                this.speed *= -1;
                break;
            case 2:
                stop = 0;
                while (elapsedtime < durTime)
                {
                    elapsedtime += Time.deltaTime;
                    yield return null;
                }
                stop = 1;
                break;
            case 3:
                sbContainer.SetActive(true);
                stop = 0;
                spacebar = 0;
                while (spacebar < maxSpacebar)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("스페이스 횟수: " + ++spacebar);
                    }
                    yield return null;
                }
                stop = 1;
                sbContainer.SetActive(false);
                break;
        }
    }
}
