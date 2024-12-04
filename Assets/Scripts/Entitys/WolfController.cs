using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;

public class WolfController : BehaviourScript
{
    Vector2 target;
    Vector2 currentPos;
    SpriteRenderer spriter;
    Animator anim;
    Rigidbody2D rigid;
    PlayerController player;
    Vector3Int wolfPos;

    Vector2 nextVec;
    Vector2 dirVec;

    bool dijkstraCheck = false;

    Tilemap tilemap;
    List<Vector3Int> path;



    int moveCount = 0;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    int roop = 0;
    void FixedUpdate()
    {
        //Debug.Log("2(" + transform.position.x + ", " + transform.position.y + ")");
        if (dijkstraCheck)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < 1.5) 
            {
                roop = 0;
                target = player.transform.position;
                currentPos = rigid.position;
            }
            else if (Vector2.Distance(transform.position, player.transform.position) < 4)
            {
                roop = 0;
                moveCount = 1;
                path.Clear();
                dijkstra(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, 0), path, 4);
                target = new Vector2(path[moveCount].x + 0.5f, path[moveCount++].y + 0.375f);
                currentPos = new Vector2(transform.position.x, transform.position.y);
            }
            else if (Vector2.Distance(target, rigid.position) < 0.1 && path.Count > 0 && moveCount < path.Count)
            {
                roop = 0;
                currentPos = new Vector2(target.x, target.y);
                target = new Vector2(path[moveCount].x + 0.5f, path[moveCount++].y + 0.375f);
                Debug.Log(moveCount + " - target: " + "(" + target.x + ", " + target.y + ")");
            }
            else if (moveCount >= path.Count)
            {
                roop = 0;
                dijkstraCheck = false;
                target = new Vector2(transform.position.x, transform.position.y);
                currentPos = new Vector2(transform.position.x, transform.position.y);
                moveCount = 0;
                path.Clear();
            }
            else {
                roop++;
                if (roop > 50)
                {
                    Debug.Log("wait");
                    currentPos = rigid.position;
                    roop = 0;
                }
            }
            if (target != currentPos)
            {
                dirVec = target - currentPos;
                nextVec = dirVec.normalized * speed * buff * Time.fixedDeltaTime * stop;
                rigid.MovePosition(rigid.position + nextVec);
            }
        }
        else
        {
            Debug.Log("다익스트라 수행");
            Debug.Log("현재 위치: (" + transform.position.x + ", " + transform.position.y + ")");
            Debug.Log("타겟 위치: (" + player.transform.position.x + ", " + player.transform.position.y + ")");
            dijkstra(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, 0), path, 10);
            target = new Vector2(path[moveCount].x + 0.5f, path[moveCount++].y + 0.375f);
            currentPos = new Vector2(transform.position.x, transform.position.y);
        }
    }

    void Start()
    {
        path = new List<Vector3Int>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        wolfPos = GameObject.Find("TileManager").GetComponent<TileController>().wolfPos;
        transform.position = new Vector3(wolfPos.x + 0.5f , wolfPos.y + 0.375f, 0);
        player = GameManager.Instance.player;
        //Debug.Log("1(" + transform.position.x + ", " + transform.position.y + ")");
    }
    void LateUpdate()
    {
        float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
        bool boolean = false;
        if ((angle > 45 && angle < 135) || (angle < -45 && angle > -135))
            boolean = false;
        else
        {
            if (speed != 0)
                boolean = true;
        }
        anim.SetBool("x", boolean);
        anim.SetFloat("y", nextVec.y);
        spriter.flipX = target.x < rigid.position.x;
    }
    public class PriorityQueue
    {
        List<Ver> Heap;
        public PriorityQueue()
        {
            Heap = new List<Ver>();
        }
        public void Enqueue(Ver data)
        {
            Heap.Add(data);
            int index = Heap.Count() - 1;
            Debug.Log("while Enqueue");
            while (index > 0)
            {
                int child = index;
                int parant = (child - 1) / 2;
                if (Heap[child].dist < Heap[parant].dist)
                {
                    Ver temp = Heap[child];
                    Heap[child] = Heap[parant];
                    Heap[parant] = temp;
                    index = parant;
                }
                else break;
            }
        }
        public Ver Dequeue()
        {
            if (Heap.Count == 0)
            {
                Debug.Log("자료가 없습니다");
            }
            Ver data = Heap[0];
            Heap[0] = Heap[Heap.Count - 1];
            Heap.RemoveAt(Heap.Count - 1);

            int parant = 0;
            int index = Heap.Count() - 1;
            Debug.Log("while Dequeue");
            while (parant <= index)
            {
                int child = (parant * 2) + 1;
                if (child > index) break;
                if (child + 1 <= index && Heap[child].dist > Heap[child + 1].dist) child++;
                if (Heap[parant].dist > Heap[child].dist)
                {
                    Ver temp = Heap[parant];
                    Heap[parant] = Heap[child];
                    Heap[child] = temp;
                    parant = child;
                }
                else break;
            }
            return data;
        }
        public bool IsEmpty()
        {
            if (Heap.Count == 0) return true;
            else return false;
        }
        public void Clear()
        {
            Heap.Clear();
        }
    }
    void dijkstra(Vector3Int start, Vector3Int end, List<Vector3Int> path, int offset = 0)
    {
        //Debug.Log("다익스트라 수행 중");
        int[] dx = new int[4] { -1, 1, 0, 0 };
        int[] dy = new int[4] { 0, 0, -1, 1 };

        int rows = Constants.sizex * 2;
        int cols = end.y - start.y + offset * 2 + 1;

        PriorityQueue minHeap = new PriorityQueue();
        bool[,] visited = new bool[rows, cols];
        int[,] distance = new int[rows, cols];
        bool[,] map = new bool[rows, cols];
        for (int j = 0; j < cols; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                visited[i, j] = false;
                distance[i, j] = Constants.INF;
                map[i, j] = tilemap.HasTile(new Vector3Int(i, j + start.y - offset, 0));
            }
        }
        distance[start.x, offset] = 0;
        minHeap.Enqueue(new Ver(start.x, offset, 0));
        Debug.Log("while IsEmpty()");
        while (!minHeap.IsEmpty())
        {
            Ver current = minHeap.Dequeue();
            //Debug.Log("current: (" + current.x + ", " + current.y + ") - " + visited[current.x, current.y]);
            if (visited[current.x, current.y]) continue;
            visited[current.x, current.y] = true;

            if (current.x == end.x && current.y == end.y - start.y + offset) break;

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];
                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && map[nx, ny])
                {
                    //Debug.Log("search: (" + nx + ", " + ny + ") - " + map[nx, ny]);
                    int newDist = current.dist + 1;
                    //Debug.Log("whichMin :" + newDist + ", " + distance[nx, ny]);
                    if (newDist < distance[nx, ny])
                    {
                        //Debug.Log("dist: " + newDist);
                        distance[nx, ny] = newDist;
                        minHeap.Enqueue(new Ver(nx, ny, newDist));
                    }
                }
            }
        }
        Vector3Int t = new Vector3Int(end.x, end.y - start.y + offset, 0);
        Debug.Log("while distance[,]");
        int roopdist = 0;
        while (distance[t.x, t.y] != 0 && roopdist < 100000)
        {
            roopdist++;
            path.Insert(0, new Vector3Int(t.x, t.y + start.y - offset, 0));
            for (int i = 0; i < 4; i++)
            {
                int nx = t.x - dx[i];
                int ny = t.y - dy[i];
                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && distance[t.x, t.y] == distance[nx, ny] + 1)
                {
                    t.x = nx;
                    t.y = ny;
                    break;
                }
            }
            if(roopdist >= 100000) Debug.Log("roopdist over");
        }
        path.Insert(0, start);

        dijkstraCheck = true;
        minHeap.Clear();
        Debug.Log("다익스트라 수행 끝");
    }
}
