using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Touch touch;
    private Vector2 startPosition, startWorldPos;
    private Vector2 endPosition;
    private Transform block;
    private Transform emptyBlock;
    private Map map;
    private Vector2 fingerPos;
    public static InputManager Instance;

    private void Awake()
    {
        Instance = this;
        map = GetComponent<Map>();
    }
    public void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            emptyBlock = map.getEmptyAreaTransform();
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                startWorldPos = Camera.main.ScreenToWorldPoint(startPosition);
                startWorldPos.x = Mathf.FloorToInt(startWorldPos.x);
                startWorldPos.y = Mathf.FloorToInt(startWorldPos.y);
                if (startWorldPos == (Vector2)emptyBlock.position) return;
                if (startWorldPos.x > map.column - 1 || startWorldPos.x < 0 ||
                 startWorldPos.y > map.row - 1 || startWorldPos.y < 0) return;
                block = map.moveBlock(startWorldPos) != null ? map.moveBlock(startWorldPos) : null;
                if (block == null) return;
            }

            fingerPos = touch.position;
            var worlPos = Camera.main.ScreenToWorldPoint(fingerPos);
            worlPos.x = Mathf.FloorToInt(worlPos.x);
            worlPos.y = Mathf.FloorToInt(worlPos.y);
            if ((Vector2)worlPos == (Vector2)emptyBlock.position)
            {
                endPosition = fingerPos;
            }


            if (touch.phase == TouchPhase.Ended)
            {
                //endPosition = touch.position;
                // var worlPos = Camera.main.ScreenToWorldPoint(endPosition);
                // worlPos.x = Mathf.FloorToInt(worlPos.x);
                // worlPos.y = Mathf.FloorToInt(worlPos.y);
                //if ((Vector2)worlPos != (Vector2)emptyBlock.position) return;
                if (block == null) return;
                var temp = block.position;
                float x = endPosition.x - startPosition.x;
                float y = endPosition.y - startPosition.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0) moveControl(Vector2.left);
                    if (x > 0) moveControl(Vector2.right);
                }
                else
                {
                    if (y > 0) moveControl(Vector2.up);
                    if (y < 0) moveControl(Vector2.down);
                }
                if (block.position != temp) emptyBlock.position = startWorldPos;
            }
        }
        map.isFinished();
    }

    private void moveControl(Vector2 vector2)
    {

        block.position = (Vector2)(emptyBlock.position - block.position) == vector2 ?
         (Vector2)block.position + vector2 : block.position;
    }
}
