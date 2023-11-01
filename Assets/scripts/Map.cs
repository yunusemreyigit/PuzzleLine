using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int row = 3, column = 3;
    int[,] mapLogic;
    Block[,] mapInterface;
    [SerializeField] private Block block;
    [SerializeField] private Block curveblock;
    private Vector2 startPoint;
    private Vector2 endPoint;
    public Color color;
    public Vector2 point;
    bool isStartExist, isEndExist;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            createMap();
        if (Input.GetKeyDown(KeyCode.W))
            createPuzzle();
    }
    // Creates map interfaces
    [ContextMenu(nameof(createMap))]
    private void createMap()
    {
        createMapLogic();
        movePointer();
        mapInterface = new Block[row, column];
        var parent = new GameObject("Blocks");
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < column; j++)
            {
                var instance = Instantiate(block, new Vector2(i, j), Quaternion.identity);
                instance.name = i + "," + j;
                if (mapLogic[i, j] == 1)
                {
                    instance.GetComponent<SpriteRenderer>().color = color;
                }
                instance.transform.SetParent(parent.transform);
                mapInterface[i, j] = instance;
            }
        }
        cameraPosition();
    }
    // Deletes maplogic and creates new maplogic
    // Deletes blocks objects
    private void remakeMapLogic()
    {
        var parent = GameObject.Find("Blocks");
        Destroy(parent);
        createMap();
    }
    // Creates an array with all values 0
    // and selects a start point that on the edge of the array
    private void createMapLogic()
    {
        isStartExist = false;
        isEndExist = false;
        mapLogic = new int[row, column];
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < column; j++)
            {
                mapLogic[i, j] = 0;

                if ((j == (column - 1) || j == 0) || (i == 0 || i == (row - 1)))
                {
                    int val = 10;
                    int rand = UnityEngine.Random.Range(0, val);
                    if (!isStartExist && rand == 1)
                    {
                        startPoint = new Vector2(i, j);
                        Debug.Log("Start Point : " + startPoint);
                        isStartExist = true;
                        val--;
                        continue;
                    }
                    if (!isEndExist && rand == 2)
                    {
                        endPoint = new Vector2(i, j);
                        Debug.Log("End Point : " + endPoint);
                        isEndExist = true;
                        val--;
                        continue;
                    }
                }
            }
        }
    }
    private void cameraPosition()
    {

        float x = (row - 1) / 2.0f;
        float y = (column - 1) / 2.0f;
        Camera.main.transform.position = new Vector3(x, y, -10);
    }
    [ContextMenu(nameof(createPuzzle))]
    private void createPuzzle()
    {
        Block curveblock = this.curveblock;
        Vector2 start = startPoint;
        Vector2 end = endPoint;
        point = start;
        while (point != end)
        {
            if ((int)(point.y - 1) >= 0 && (int)(point.y - 1) < row && mapLogic[(int)point.x, (int)(point.y - 1)] == 0)//&& (int)point.y - 1 < mapLogic.GetLength(1))
            {
                point.y -= 1;
                Debug.Log(point);
                mapLogic[(int)point.x, (int)(point.y)] = 1;
                mapInterface[(int)point.x, (int)(point.y)].GetComponent<SpriteRenderer>().color = color;
                mapInterface[(int)point.x, (int)(point.y)].name = "1";
            }
            else if (mapLogic[(int)point.x, (int)(point.y + 1)] == 0 && (int)(point.y + 1) >= 0 && (int)(point.y + 1) < row)// && (int)point.y + 1 < mapLogic.GetLength(1))
            {
                point.y += 1;
                Debug.Log(point);
                mapLogic[(int)point.x, (int)(point.y)] = 1;
                mapInterface[(int)point.x, (int)(point.y)].GetComponent<SpriteRenderer>().color = color;
                mapInterface[(int)point.x, (int)(point.y)].name = "1";

            }
            else if (mapLogic[(int)point.x - 1, (int)(point.y)] == 0 && (int)(point.x - 1) >= 0 && (int)(point.x - 1) < row)//&& (int)point.x - 1 < mapLogic.GetLength(2))
            {
                point.x -= 1;
                Debug.Log(point);
                mapLogic[(int)point.x, (int)(point.y)] = 1;
                mapInterface[(int)point.x, (int)(point.y)].GetComponent<SpriteRenderer>().color = color;
                mapInterface[(int)point.x, (int)(point.y)].name = "1";


            }
            else if (mapLogic[(int)point.x + 1, (int)(point.y)] == 0 && (int)(point.x + 1) >= 0 && (int)(point.x + 1) < row)// && point.x + 1 < mapLogic.GetLength(row))
            {
                point.x += 1;
                Debug.Log(point);
                mapLogic[(int)point.x, (int)(point.y)] = 1;
                mapInterface[(int)point.x, (int)(point.y)].GetComponent<SpriteRenderer>().color = color;
                mapInterface[(int)point.x, (int)(point.y)].name = "1";

            }
        }

    }
    // Creates a path for puzzle
    void movePointer()
    {
        Vector2 pointer = startPoint;
        int x = (int)pointer.x;
        int y = (int)pointer.y;
        List<Vector2> dir = new List<Vector2>();
        int counter = 0;
        mapLogic[(int)startPoint.x, (int)startPoint.y] = 1;
        while (pointer != endPoint)
        {
            if (isInMap(y + 1, 1) && mapLogic[x, y + 1] == 0)    //TOP
            {
                dir.Add(new Vector2(x, y + 1));
            }
            if (isInMap(y - 1, 1) && mapLogic[x, y - 1] == 0)    //BOTTOM
            {
                dir.Add(new Vector2(x, y - 1));
            }
            if (isInMap(x + 1, 0) && mapLogic[x + 1, y] == 0)    //RIGHT
            {
                dir.Add(new Vector2(x + 1, y));
            }
            if (isInMap(x - 1, 0) && mapLogic[x - 1, y] == 0)    //LEFT
            {
                dir.Add(new Vector2(x - 1, y));
            }

            int count = dir.Count;
            if (count == 0)
            {
                remakeMapLogic();
                return;
            }
            int v = UnityEngine.Random.Range(0, count);
            // Debug.Log(v + ":" + dir.Count);
            pointer = dir[v];
            x = (int)pointer.x;
            y = (int)pointer.y;
            mapLogic[x, y] = 1;
            // Debug.Log(pointer + ":" + mapLogic[x, y]);
            dir.Clear();
            counter++;
            // if (counter >= 7)
            //     break;
        }
    }
    //Returns true if value is lower than mapLogic array dimension length.
    bool isInMap(int val, int dim)
    {
        return val < mapLogic.GetLength(dim) && val >= 0;
    }
}
