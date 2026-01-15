using UnityEngine;

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

    public void PlayAnim(){}

}
