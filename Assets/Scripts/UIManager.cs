using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject cloak;
    public List<Image> cloaks;
    public int MaxCloak;
    public Slider gauge;
    public Transform cloakCont;

    PlayerController player;
    WolfController wolf;
    GameObject sbCont;
    GameObject distance;
    GameObject time;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
        wolf = GameManager.Instance.wolf;
        sbCont = GameObject.Find("SpacebarContainer");
        distance = GameObject.Find("Distance");
        time = GameObject.Find("Time");
        for (int i = 0; i < 10; i++)
        {
            GameObject c = Instantiate(cloak, cloakCont);
            cloaks.Add(c.GetComponent<Image>());
        }
        gauge = GameObject.Find("SbGauge").GetComponent<Slider>();
        gauge.minValue = 0;
        gauge.maxValue = player.maxSpacebar;
        sbCont.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cloaks.Count; i++)
        {
            if(i < player.Cloak)
            {
                cloaks[i].enabled = true;
            }else
            {
                cloaks[i].enabled = false;
            }
        }

        gauge.value = player.spacebar;
        this.distance.GetComponent<TextMeshProUGUI>().text = "Distance: " + Vector2.Distance(wolf.transform.position, player.transform.position).ToString("F1");
        this.time.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.time.ToString("F1");
    }
}
