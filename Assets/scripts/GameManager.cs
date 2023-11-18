using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    private int level = 0;
    [SerializeField] private int requiredXp;
    [SerializeField] private int currentXp;
    private float minute = 0, second = 0;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        dataObject data = FileManager.Instance.loadFromJSON("data");
        level = data.level;
        requiredXp = calculateRequiredXp(level);
        currentXp = data.xp;
    }
    private void Update()
    {
        GameTimer();
    }
    private int calculateRequiredXp(int level)
    {
        return (level + 1) * 7;
    }
    public void addXp(int value)
    {
        currentXp += value;
        FileManager.Instance.saveToJSON(new dataObject(currentXp, level), "data");
    }
    public int getXp()
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
        FileManager.Instance.saveToJSON(new dataObject(getXp(), getLevel()), "data");
    }
    public int getLevel()
    {
        return level;
    }
    public void GameTimer()
    {
        second += Time.deltaTime;
        if (second > 60)
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
