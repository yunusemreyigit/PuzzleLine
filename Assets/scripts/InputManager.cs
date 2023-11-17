using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Touch touch;
    private Touch touch1;
    private Vector2 startPosition, startWorldPos;
    private Transform block;
    private Transform emptyBlock;
    private Map map;
    private Vector2 fingerPos;
    public static InputManager Instance;

    Vector2 blockTemp;
    private Vector2 emptyBlockTemp;
    private float timer = 0;
    public float animSpeed = 1;
    private bool isTouched = false;

    private float distance = 0;
    Vector2 moveStartPosition;

    private void Awake()
    {
        Instance = this;
        map = GetComponent<Map>();
    }
    public void Update()
    {
        if (Input.touchCount == 2)
        {
            touch = Input.GetTouch(0);
            touch1 = Input.GetTouch(1);

            var pos1 = touch.position;
            var pos2 = touch1.position;

            float currentDistance = Vector2.Distance(pos1, pos2);
            if (touch.phase == TouchPhase.Began && touch1.phase == TouchPhase.Began)
            {
                distance = currentDistance;
                moveStartPosition = pos1;
            }
            if (touch.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                if (Mathf.Abs(currentDistance - distance) >= 200)
                {

                    if (currentDistance < distance && Camera.main.orthographicSize <= map.column + 2)
                    {
                        Camera.main.orthographicSize += 6 * Time.deltaTime;
                    }
                    else if (currentDistance > distance && Camera.main.orthographicSize > 5)
                    {
                        Camera.main.orthographicSize -= 6 * Time.deltaTime;
                    }
                }
                else
                {
                    if (map.column <= 4 || map.row <= 4) return;
                    var posCam = Camera.main.transform.position;
                    Vector2 diff = (moveStartPosition - pos1).normalized;
                    var xMax = map.column - 1.5f > 1 ? map.column - 1.5f : 1;
                    posCam.x = Mathf.Clamp(posCam.x, 1, xMax);
                    var yMax = map.row - 3 > 3 ? map.row - 3 : 3;
                    posCam.y = Mathf.Clamp(posCam.y, 3, yMax);
                    Camera.main.transform.position = new Vector3(posCam.x + diff.x * Time.deltaTime * 5,
                    posCam.y + diff.y * Time.deltaTime * 5, -10);
                }
            }
            if (touch.phase == TouchPhase.Stationary && touch1.phase == TouchPhase.Stationary)
            {
                moveStartPosition = pos1;
                distance = currentDistance;

            }
        }
        if (Input.touchCount > 0 && isTouched == false)
        {
            touch = Input.GetTouch(0);
            emptyBlock = map.getEmptyAreaTransform();
            emptyBlockTemp = emptyBlock.position;

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                startWorldPos = pixelPosToWorldPos(startPosition);
                if (isTouchedOutsideBlock(startWorldPos, emptyBlock.position)) return;
                // emptyBlockTemp = emptyBlock.position;
                block = map.moveBlock(startWorldPos);
                blockTemp = startWorldPos;

            }
            if (touch.phase == TouchPhase.Moved)
            {
                fingerPos = touch.position;
                Vector2 worlPos = pixelPosToWorldPos(fingerPos);
                if (worlPos == (Vector2)emptyBlock.position)
                {
                    Touched();
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (block != null) block = null;
                var endpos = touch.position;
                float x = endpos.x - startPosition.x;
                float y = endpos.y - startPosition.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x > 0) changePosEmptyAreaWithoutTouch(Vector2.left);
                    if (x < 0) changePosEmptyAreaWithoutTouch(Vector2.right);
                }
                else
                {
                    if (y < 0) changePosEmptyAreaWithoutTouch(Vector2.up);
                    if (y > 0) changePosEmptyAreaWithoutTouch(Vector2.down);
                }
            }

        }
        if (timer >= 1)
        {
            block.position = emptyBlockTemp;
            emptyBlock.position = startWorldPos;
            blockTemp = block.position;
            timer = 0;
            isTouched = false;
            block = null;
        }
        if (block == null)
        {
            isTouched = false;
            return;
        }


        if (isTouched && Vector2.Distance((Vector2)block.position, emptyBlockTemp) <= 1)
        {
            SoundManager.Instance.playSfx("Block");
            timer += Time.deltaTime * animSpeed;
            block.position = Vector2.Lerp(blockTemp, emptyBlockTemp, animTime(timer));
            block.localScale = Vector2.Lerp(new Vector2(.5f, .5f), Vector2.one, animTime(timer / 2));
            emptyBlock.position = Vector2.Lerp(emptyBlockTemp, blockTemp, animTime(timer));
        }
        else
        {
            isTouched = false;
        }

        if (map.isFinished())
        {
            timer = 0;
            isTouched = false;
            block = null;
        }

    }

    private void changePosEmptyAreaWithoutTouch(Vector2 vector2)
    {
        if (blockTemp - (Vector2)emptyBlock.position == vector2)
        {
            Touched();
        }
    }

    private void Touched()
    {
        if (block != null)
            isTouched = true;
    }

    private Vector2 pixelPosToWorldPos(Vector2 pixelPos)
    {
        var worldPos = Camera.main.ScreenToWorldPoint(pixelPos);
        worldPos.x = Mathf.FloorToInt(worldPos.x);
        worldPos.y = Mathf.FloorToInt(worldPos.y);
        return worldPos;
    }

    private bool isTouchedOutsideBlock(Vector2 start, Vector2 emptyArea)
    {
        return
        start == emptyArea ||
        startWorldPos.x > map.column - 1 ||
        startWorldPos.x < 0 ||
        startWorldPos.y > map.row - 1 ||
        startWorldPos.y < 0;
    }

    private float animTime(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }

}
