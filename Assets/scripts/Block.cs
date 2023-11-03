using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{

    public int blockCounter = 0;

    public int getBlockCounter()
    {
        return blockCounter;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            blockCounter++;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            blockCounter--;
        }

    }

}
