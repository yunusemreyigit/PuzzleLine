using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{

    public int blockCounter = 0;
    public List<Block> neighbourBlocks;
    public float animSeconds = 0;
    public bool activeBlock;

    public int getBlockCounter()
    {
        return blockCounter;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            blockCounter++;
            if (activeBlock)
                neighbourBlocks.Add((Block)other.GetComponent("Block"));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            blockCounter--;
            if (activeBlock)
                neighbourBlocks.Remove((Block)other.GetComponent("Block"));
        }

    }

    public bool PlayAnim(){
        if (activeBlock == false) return false;
        if (animSeconds >= 1.0f)
        {
            if (neighbourBlocks.First())
            {
                neighbourBlocks.First().neighbourBlocks.Remove(this);
                return neighbourBlocks.First().PlayAnim();
            }
            else
                return true;
        }
        else
        {
        animSeconds += Time.deltaTime;
        GetComponent<SpriteRenderer>().material.SetFloat("_BlockProcess",  animSeconds);
        }
        return false;
    }

}
