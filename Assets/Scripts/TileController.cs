using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using static GameManager;

public class TileController : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap grassmap;
    public Tilemap treemap;
    public TileBase load;
    public TileBase grass;
    public TileBase tree;

    public Vector3Int playerPos;
    public Vector3Int wolfPos;

    public GameObject[] prefabs;

    void GetPath(ref GraphType g, int s, int e)
    {
        if (g.next[s, e] != -1)
        {
            g.path.Add(s);
            while (s != e)
            {
                s = g.next[s, e];
                g.path.Add(s);
            }
            g.path.Add(e);
        }
        else
        {
            Debug.Log("경로 생성 불가");
        }
    }

    void DrawBG(GraphType g, int y)
    {
        for (int j = g.Vers[0].y - 1; j < g.Vers[g.Vers.Length - 1].y + 1; j++)
        {
            for (int i = 0; i < Constants.sizex * 2 + 20; i++)
            {
                grassmap.SetTile(new Vector3Int(i - 10, j + y, 0), grass);
                treemap.SetTile(new Vector3Int(i - 10, j + y, 0), tree);
            }
        }
    }

    void DrawPath(ref GraphType g, int start, int end, int y)
    {
        GetPath(ref g, start, end);
        g.maps.Add(new Map(g.Vers[start].x, g.Vers[start].y - 1));
        g.maps.Add(new Map(g.Vers[g.path[0]].x, g.Vers[g.path[0]].y));
        for (int p = 1; p < g.path.Count; p++)
        {
            g.maps.Add(new Map((g.Vers[g.path[p - 1]].x + g.Vers[g.path[p]].x) / 2, (g.Vers[g.path[p - 1]].y + g.Vers[g.path[p]].y) / 2));
            g.maps.Add(new Map(g.Vers[g.path[p]].x, g.Vers[g.path[p]].y));
        }
        foreach (Map map in g.maps)
        {
            tilemap.SetTile(new Vector3Int(map.x, map.y + y, 0), load);
            treemap.SetTile(new Vector3Int(map.x, map.y + y, 0), null);
        }
        g.path.Clear();
        g.maps.Clear();
    }
    void Block(ref GraphType g, ref block b)
    {
        List<int> endlist = new List<int>();
        int d = 0;
        if(b.blockCount != 0) b.y += g.Vers[b.end].y + 1;
        DrawBG(g, b.y);
        while (d < b.pathCount)
        {
            b.end = Random.Range(Constants.sizex * (Constants.sizey - 1), Constants.sizex * Constants.sizey);
            DrawPath(ref g, b.start[d], b.end, b.y);
            if (b.y == 0)
            {
                playerPos = new Vector3Int(g.Vers[b.end].x, g.Vers[b.end].y, 0);
                wolfPos = new Vector3Int(g.Vers[0].x, g.Vers[0].y - 1, 0);
                treemap.SetTile(new Vector3Int(g.Vers[0].x, g.Vers[0].y - 2, 0), tree);
            }
            if (!endlist.Contains(b.end) && b.y != 0)
            {
                int p = Random.Range(0, prefabs.Length);
                var prefab = Instantiate(prefabs[p]);
                prefab.transform.position = new Vector3Int(g.Vers[b.end].x, g.Vers[b.end].y + b.y, 0);
            }
            endlist.Add(b.end);
            b.start[d++] = b.end - (Constants.sizex * (Constants.sizey - 1));
        }
        b.blockCount++;
    }

    void Start()
    {
        for (int j = -10; j < 0; j++)
        {
            for (int i = 0; i < Constants.sizex * 2 + 20; i++)
            {
                grassmap.SetTile(new Vector3Int(i - 10, j, 0), grass);
                treemap.SetTile(new Vector3Int(i - 10, j, 0), tree);
            }
        }
        for (int i = 0; i < 3; i++) 
            Block(ref GameManager.g, ref GameManager.b);
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.player.transform.position.y >= GameManager.b.y - Constants.sizey * 2 + playerPos.y)
            Block(ref GameManager.g, ref GameManager.b);
        for (int i = 0; i < Constants.sizex * 2; i++) {
            Vector3Int v = new Vector3Int(i, (int)GameObject.Find("Wolf").transform.position.y - Constants.sizey * 2, 0);
            if (!treemap.HasTile(v)) treemap.SetTile(v, tree);
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Item"))
        {
            if(g.transform.position.y < GameObject.Find("Wolf").transform.position.y - Constants.sizey * 2) Destroy(g);
        }
    }

    public GameObject chack;
}
