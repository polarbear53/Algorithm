using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static TileController;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public PlayerController player;
    public WolfController wolf;

    public static GraphType g;
    public static block b;

    public float time;
    public static class Constants
    {
        public const int sizex = 20;
        public const int sizey = 5;
        public const int INF = 100000;
    }
    public class block
    {
        public int blockCount;
        public int end;
        public int y;
        public int pathCount;
        public int[] start;
        public block()
        {
            blockCount = 0;
            end = Random.Range(Constants.sizex * (Constants.sizey - 1), Constants.sizex * Constants.sizey);
            y = 0;
            pathCount = 3;
            start = new int[pathCount];
        }
    }
    public class Ver
    {
        public int x;
        public int y;
        public int dist;
        public Ver(int j, int i, int dist = Constants.INF)
        {
            x = j;
            y = i;
            this.dist = dist;
        }
    }
    public class Map
    {
        public int x;
        public int y;
        public Map(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class GraphType
    {
        public Ver[] Vers;
        public int[,] dist;
        public int[,] next;
        public List<Map> maps;
        public List<int> path;

        public GraphType()
        {
            Vers = new Ver[Constants.sizex * Constants.sizey];
            dist = new int[Constants.sizex * Constants.sizey, Constants.sizex * Constants.sizey];
            next = new int[Constants.sizex * Constants.sizey, Constants.sizex * Constants.sizey];
            maps = new List<Map>();
            path = new List<int>();
        }
    }
    void AllPairShortest(ref GraphType g)
    {
        for (int k = 0; k < g.Vers.Length; k++)
        {
            for (int i = 0; i < g.Vers.Length; i++)
            {
                for (int j = 0; j < g.Vers.Length; j++)
                {
                    if (g.dist[i, k] != Constants.INF && g.dist[k, j] != Constants.INF && g.dist[i, j] > g.dist[i, k] + g.dist[k, j])
                    {
                        g.dist[i, j] = g.dist[i, k] + g.dist[k, j];
                        g.next[i, j] = g.next[i, k];
                    }
                }
            }
        }
    }

    void Awake()
    {
        GameManager.Instance = this;
        g = new GraphType();
        b = new block();
        int VerIndex = 0;
        for (int i = 0; i < Constants.sizey * 2 + 1; i++)
        {                            //정점 설정
            for (int j = 0; j < Constants.sizex * 2 + 1; j++)
            {

                if (i % 2 != 0 && j % 2 != 0)
                {
                    g.Vers[VerIndex++] = new Ver(j, i);
                }
            }
        }
        for (int i = 0; i < Constants.sizex * Constants.sizey; i++)
        {
            for (int j = 0; j < Constants.sizex * Constants.sizey; j++)
            {
                g.dist[i, j] = Constants.INF;
                g.next[i, j] = -1;
            }
        }
        for (int i = 0; i < VerIndex; i++)                                 //간선 설정
        {
            if (i % Constants.sizex != Constants.sizex - 1)
            {
                g.dist[i, i + 1] = Random.Range(0, 60);
                g.dist[i + 1, i] = Random.Range(0, 60);
                g.next[i, i + 1] = i + 1;
                g.next[i + 1, i] = i;
            }
            if (i / Constants.sizex != Constants.sizey - 1)
            {
                g.dist[i, i + Constants.sizex] = Random.Range(40, 100);
                g.dist[i + Constants.sizex, i] = Random.Range(40, 100);
                g.next[i, i + Constants.sizex] = i + Constants.sizex;
                g.next[i + Constants.sizex, i] = i;
            }
        }
        AllPairShortest(ref g);
    }
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (Vector2.Distance(wolf.transform.position, player.transform.position) < 1)
        {
            Debug.Log("gameover");
            SceneManager.LoadScene("GameOver");
        }
        if (time > 180)
        {
            Debug.Log("endingscene");
            SceneManager.LoadScene("EndingScene");
        }
    }
}
