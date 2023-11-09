using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int coin = 0;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

    }
    public void addCoin(int coinAmount)
    {
        coin += coinAmount;
    }
    public int getCoinAmount()
    {
        return coin;
    }
    void Start()
    {
        coin = 0;
    }
}
