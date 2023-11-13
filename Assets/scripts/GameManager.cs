using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    private int level = 0;
    [SerializeField] private float requiredXp;
    [SerializeField] private float currentXp;
    private float minute = 0, second = 0;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        requiredXp = calculateRequiredXp(level);
    }
    private void Update()
    {
        GameTimer();
    }
    private float calculateRequiredXp(int level)
    {
        return (level + 1) * 7;
    }
    public void addXp(float value)
    {
        currentXp += value;
    }
    public void setXp(float xp)
    {
        currentXp = xp;
    }
    public float getXp()
    {
        return currentXp;
    }
    public float getRequiredXp()
    {
        return requiredXp;
    }
    public void addLevel()
    {
        if (currentXp >= requiredXp)
        {
            level++;
            currentXp = currentXp - requiredXp;
            requiredXp = calculateRequiredXp(level);
        }
    }
    public int getLevel()
    {
        return level;
    }
    public void GameTimer()
    {
        second += Time.deltaTime;
        if (second >= 60)
        {
            minute++;
            second = 0;
        }
    }
    public float getSecond()
    {
        return second;
    }
    public float getMinute() { return minute; }
    public void resetTimer() { minute = 0; second = 0; }
}
