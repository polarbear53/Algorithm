using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    protected PlayerController player;
    protected float speed;
    protected float durTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("æ∆¿Ã≈€ »πµÊ");
            Destroy(gameObject);
            ItemGet();
        }
    }
    protected virtual void ItemGet() { }
}
