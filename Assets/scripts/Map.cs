using System;
using System.Collections.Generic;
using UnityEngine;
/*
0 - Empty Bloc
7 - Start End Block
8 - Empty Area
*/
public class Map : MonoBehaviour
{
    public int column = 3, row = 3;
    int[,] mapLogic;
    [SerializeField] private Block[] blocks;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 startPointMap, endPointMap;
    bool isStartExist, isEndExist;
    List<Vector2> solutionMap;
    private List<Block> blockList;
    private GameObject emptyObject;
    private Block startBlock, endBlock;
    private void Start()
    {
        startBlock = Instantiate(blocks[7]);
        endBlock = Instantiate(blocks[7]);
        solutionMap = new List<Vector2>();
        blockList = new List<Block>();
        initializeMethods();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            initializeMethods();
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector2 v1 = new Vector2(1, 1);
            Vector2 v2 = new Vector2(2, 1);
            Vector2 v3 = new Vector2(2, 2);

            Debug.Log(v2 - v1);
            Debug.Log(v3 - v2);
            if ((v2 - v1) == Vector2.right)
            {
                (v2 - v1).Normalize();
                Debug.Log("right");
            }
            if ((v3 - v2) == Vector2.up)
            {
                Debug.Log("up");
            }

        }

    }
    public void setSize(int r, int c)
    {
        row = r;
        column = c;
    }
    public Transform getEmptyAreaTransform()
    {
        return emptyObject.transform;
    }
    [ContextMenu(nameof(initializeMethods))]
    void initializeMethods()
    {
        solutionMap.Clear();
        createMapLogic();
        movePointer();
        createBlocks();
        shuffleMap();
        createMap();    //interface
        cameraPosition();
        startBlock.transform.position = startPointMap;
        endBlock.transform.position = endPointMap;
    }
    public void startGame()
    {
        blockList.Clear();
        initializeMethods();
    }
    public void restartScene()
    {
        findParentDestroy();
        shuffleMap();
        blockList.Clear();
        createMap();    //interface
        cameraPosition();
        startBlock.transform.position = startPointMap;
        endBlock.transform.position = endPointMap;
    }
    public void isFinished()
    {
        foreach (var item in blockList)
        {
            if (item.name != "0")
            {
                if (item.blockCounter != 2) return;
            }
        }
        GameManager.Instance.addCoin(solutionMap.Count);
        startGame();
        Debug.Log("Game Over Successfully !");
    }
    public Transform moveBlock(Vector2 position)
    {
        Block block = null;
        // TODO: sınırlar ile kontrol edilebilir
        foreach (var item in blockList)
        {
            if ((Vector2)item.transform.position == position)
            {
                block = item;
            }
        }
        return block.transform;
    }
    // Creates map interfaces
    private void createMap()
    {
        findParentDestroy();
        GameObject parent = new GameObject("Blocks");
        for (var i = 0; i < column; i++)
        {
            for (var j = 0; j < row; j++)
            {
                Block block = blockDecider(mapLogic[i, j], i, j);
                if (mapLogic[i, j] == 8)
                {
                    emptyObject = block.gameObject;
                }
                block.transform.SetParent(parent.transform);
            }
        }
    }

    private void findParentDestroy()
    {
        var parent = GameObject.Find("Blocks");
        if (parent != null) Destroy(parent);
    }

    // Deletes maplogic and creates new maplogic
    // Deletes blocks objects
    private void remakeMapPointer()
    {
        solutionMap.Clear();
        blockList.Clear();
        mapLogicClear();
        movePointer();
    }
    // Creates an array with all values 0
    // and selects a start point that on the edge of the array
    private void createMapLogic()
    {
        isStartExist = false;
        isEndExist = false;
        mapLogic = new int[column, row];
        mapLogicClear();
        while (!isStartExist)
        {
            int i = UnityEngine.Random.value < .5f ? 0 : column - 1;
            int j = UnityEngine.Random.value < .5f ? 0 : row - 1;
            if (i == 0 && j == 0) continue;
            startPoint = new Vector2(i, j);
            if (j == (row - 1) && (i != 0 || i == (column - 1)))
            {
                startPointMap = new Vector2(i, j + 1);
            }
            else if (j == 0 && (i != 0 || i == (column - 1)))
            {
                startPointMap = new Vector2(i, j - 1);
            }
            else if (i == 0)
            {
                startPointMap = new Vector2(i - 1, j);
            }
            else if (i == (column - 1))
            {
                startPointMap = new Vector2(i + 1, j);
            }
            //Debug.Log("Start Point : " + startPoint);
            // Debug.Log("Start Point Map : " + startPointMap);
            isStartExist = true;
        }
        while (!isEndExist || endPoint == startPoint)
        {
            int i = UnityEngine.Random.value < .5f ? 0 : column - 1;
            int j = UnityEngine.Random.value < .5f ? 0 : row - 1;
            if (i == 0 && j == 0) continue;
            endPoint = new Vector2(i, j);
            if (j == (row - 1) && (i != 0 || i == (column - 1)))
            {
                endPointMap = new Vector2(i, j + 1);
            }
            else if (j == 0 && (i != 0 || i == (column - 1)))
            {
                endPointMap = new Vector2(i, j - 1);
            }
            else if (i == 0)
            {
                endPointMap = new Vector2(i - 1, j);
            }
            else if (i == (column - 1))
            {
                endPointMap = new Vector2(i + 1, j);
            }
            //Debug.Log("End Point : " + endPoint);
            // Debug.Log("End Point Map : " + endPointMap);
            isEndExist = true;
        }
    }

    private void mapLogicClear()
    {
        int randx = UnityEngine.Random.Range(0, column);
        int randy = UnityEngine.Random.Range(0, row);
        for (var i = 0; i < column; i++)
        {
            for (var j = 0; j < row; j++)
            {
                mapLogic[i, j] = (i == 0 && j == 0) ? 8 : 0;
            }
        }
    }

    private void cameraPosition()
    {

        float x = column / 2.0f;
        float y = row / 2.0f;
        Camera.main.transform.position = new Vector3(x, y, -10);
    }
    Block blockDecider(int number, int x, int y)
    {
        Block block = Instantiate(blocks[number], new Vector2(x, y), Quaternion.identity);
        block.name = number.ToString();
        if (number != 8)
            blockList.Add(block);
        return block;
    }
    // Creates a path for puzzle
    void movePointer()
    {
        solutionMap.Add(startPoint);
        Vector2 pointer = startPoint;
        int x = (int)pointer.x;
        int y = (int)pointer.y;
        List<Vector2> dir = null;
        if (dir != null) { dir.Clear(); } else { dir = new List<Vector2>(); }
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
                remakeMapPointer();
                return;
            }
            int v = UnityEngine.Random.Range(0, count);
            // Debug.Log(v + ":" + dir.Count);
            pointer = dir[v];
            x = (int)pointer.x;
            y = (int)pointer.y;
            mapLogic[x, y] = 1;
            //Debug.Log(pointer);
            dir.Clear();
            solutionMap.Add(pointer);
        }
    }
    //Returns true if value is lower than mapLogic array dimension length.
    bool isInMap(int val, int dim)
    {
        return val < mapLogic.GetLength(dim) && val >= 0;
    }
    void createBlocks()
    {
        Vector2 currentPos;
        Vector2 prePos;
        Vector2 nextPos;

        for (var i = 0; i < solutionMap.Count; i++)
        {
            currentPos = solutionMap[i];
            prePos = i != 0 ? solutionMap[i - 1] : startPointMap;
            nextPos = i != solutionMap.Count - 1 ? solutionMap[i + 1] : endPointMap;
            var dir1 = prePos - currentPos; // arrival direction
            var dir2 = nextPos - currentPos; // target direction

            //Debug.Log("dir1 :" + dir1 + "   dir2 :" + dir2);
            while (true)
            {
                if (dir1 == Vector2.up)
                {
                    if (dir2 == Vector2.right)
                    {
                        mapLogic[(int)currentPos.x, (int)currentPos.y] = 3;
                        break;
                    }
                    if (dir2 == Vector2.left)
                    {
                        mapLogic[(int)currentPos.x, (int)currentPos.y] = 4;
                        break;
                    }
                    if (dir2 == Vector2.down)
                    {
                        mapLogic[(int)currentPos.x, (int)currentPos.y] = 1;
                        break;

                    }
                }
                else if (dir1 == Vector2.down)
                {
                    if (dir2 == Vector2.right)
                    {
                        mapLogic[(int)currentPos.x, (int)currentPos.y] = 6;
                        break;
                    }
                    if (dir2 == Vector2.left)
                    {
                        mapLogic[(int)currentPos.x, (int)currentPos.y] = 5;
                        break;
                    }
                }
                else if (dir1 == Vector2.right)
                {
                    if (dir2 == Vector2.left)
                    {
                        mapLogic[(int)currentPos.x, (int)currentPos.y] = 2;
                        break;
                    }
                }

                var temp = dir1;
                dir1 = dir2;
                dir2 = temp;
            }
        }
    }
    void shuffleMap()
    {
        List<int> tempArr = null;
        if (tempArr != null) { tempArr.Clear(); } else { tempArr = new List<int>(); }
        foreach (var item in mapLogic)
        {
            tempArr.Add(item);
        }
        int count = tempArr.Count;

        for (var i = 0; i < count; i++)
        {
            int tmp = tempArr[i];
            int r = UnityEngine.Random.Range(i, count);
            tempArr[i] = tempArr[r];
            tempArr[r] = tmp;
        }
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                mapLogic[i, j] = tempArr[count - 1];
                //Debug.Log("x, y, n, c: " + i + " " + j + " " + tempArr[count - 1] + " " + count);
                count--;
            }
        }
    }
}
