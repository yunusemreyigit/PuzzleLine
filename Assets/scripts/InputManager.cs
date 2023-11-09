using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Touch touch;
    private Vector2 startPosition, startWorldPos;
    private Vector2 endWorldPos;
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
    private void Awake()
    {
        Instance = this;
        map = GetComponent<Map>();
    }
    public void Update()
    {
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
                if (block == null) return;
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

        if (isTouched && Vector2.Distance((Vector2)block.position, emptyBlockTemp) <= 1)
        {
            SoundManager.Instance.playSfx("Block");
            timer += Time.deltaTime * animSpeed;
            block.position = Vector2.Lerp(blockTemp, emptyBlockTemp, animTime(timer));
            emptyBlock.position = Vector2.Lerp(emptyBlockTemp, blockTemp, animTime(timer));
        }

        if (map.isFinished())
        {
            timer = 0;
            isTouched = false;
        }

    }

    private void changePosEmptyAreaWithoutTouch(Vector2 vector2)
    {
        if ((Vector2)block.position - (Vector2)emptyBlock.position == vector2)
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
