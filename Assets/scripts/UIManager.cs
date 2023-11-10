using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button restartButton;
    public Button ThreeButton;
    public Button FourButton;
    private Map map;

    public RectTransform xpBar;
    public Text currLevel;
    public Text nextLevel;
    public GameObject instractionPanel;
    public GameObject privateGame;


    float timer;


    public Text row;
    public Text column;
    private void Awake()
    {
        map = GetComponent<Map>();
    }
    private void Update()
    {
        levelControl();
        if (timer >= 1)
        {
            timer = 0;
        }

        int v = GameManager.Instance.getLevel();
        currLevel.text = v.ToString();
        nextLevel.text = (v + 1).ToString();
    }

    private void levelControl()
    {
        timer += Time.deltaTime;
        float x = Mathf.Lerp(xpBar.localScale.x, GameManager.Instance.getXp() / GameManager.Instance.getRequiredXp(), Time.deltaTime);
        xpBar.localScale = new Vector3(x, 1, 1);
        if (x >= 1)
            GameManager.Instance.addLevel();
    }
    private float animTime(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }

    public void restartGame()
    {
        map.restartScene();
    }
    public void getPrivatePanel()
    {
        privateGame.SetActive(true);
    }
    public void randomMap()
    {
        var x = UnityEngine.Random.Range(3, 7);
        var y = UnityEngine.Random.Range(3, 7);
        map.setSize(x, y);
        map.startGame();
    }
    public void exitInstraction()
    {
        instractionPanel.SetActive(false);
    }
    public void startPrivateGame()
    {
        int r = short.Parse(row.text);
        int c = short.Parse(column.text);
        if (r > 10 || c > 10) return;
        privateGame.SetActive(false);
        map.setSize(r, c);
        map.startGame();
    }
    public void privateGameExitPanel()
    {
        privateGame.SetActive(false);
    }
}
