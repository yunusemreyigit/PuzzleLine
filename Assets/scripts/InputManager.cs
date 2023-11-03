using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Touch touch;
    public Vector2 startPosition;
    public Vector2 endPosition;

    private static InputManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void findDirection()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                endPosition = touch.position;

                float x = endPosition.x - startPosition.x;
                float y = endPosition.y - startPosition.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0) Debug.Log("left");
                    if (x > 0) Debug.Log("right");
                }
                else
                {
                    if (y > 0) Debug.Log("up");
                    if (y < 0) Debug.Log("down");
                }
            }
        }
    }
}
