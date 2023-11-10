using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int level = 0;
    [SerializeField] private float requiredXp;
    [SerializeField] private float currentXp;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        requiredXp = calculateRequiredXp(level);
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

}
