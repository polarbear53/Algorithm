using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BehaviourScript
{
    public Vector2 inputVec;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    Vector2 nextVec;
    WolfController wolf;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        wolf = GameManager.Instance.wolf;
        Vector3Int playerPos = GameObject.Find("TileManager").GetComponent<TileController>().playerPos;
        transform.position = new Vector3(playerPos.x + 0.5f, playerPos.y + 0.75f, 0);
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        nextVec = inputVec.normalized * speed * buff * Time.fixedDeltaTime * stop;
        rigid.MovePosition(rigid.position + nextVec);
        
    }

    void LateUpdate()
    {
        anim.SetFloat("y", nextVec.y);

        if (nextVec.x != 0)
        {
            anim.SetBool("x", true);
            spriter.flipX = nextVec.x < 0;
        }
        else anim.SetBool("x", false);
    }
}
