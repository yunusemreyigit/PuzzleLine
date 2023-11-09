using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Touch touch;
    private Vector2 startPosition, startWorldPos;
    private Transform block;
    private Transform emptyBlock;
    private Map map;
    private Vector2 fingerPos;
    public static InputManager Instance;

    Vector2 blockTemp;
    private Vector2 emptyBlockTemp;
    public float timer = 0;
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
                    if (block != null)
                        isTouched = true;

                }
            }

        }
        if (timer >= 1)
        {
            block.position = emptyBlock.position;
            timer = 0;
            isTouched = false;
            emptyBlock.position = startWorldPos;
            block = null;
        }

        if (isTouched && Vector2.Distance((Vector2)block.position, emptyBlockTemp) <= 1)
        {
            timer += Time.deltaTime * animSpeed;
            block.position = Vector2.Lerp(blockTemp, emptyBlockTemp, animTime(timer));
        }

        if (map.isFinished())
        {
            timer = 0;
            isTouched = false;
        }

    }

    private Vector2 pixelPosToWorldPos(Vector2 pixelPos)
    {
        var worlPos = Camera.main.ScreenToWorldPoint(pixelPos);
        worlPos.x = Mathf.FloorToInt(worlPos.x);
        worlPos.y = Mathf.FloorToInt(worlPos.y);
        return worlPos;
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
