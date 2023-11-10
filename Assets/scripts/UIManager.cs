using System.Runtime.InteropServices;
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
    float timer;
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
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.addXp(50);

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
    public void get3x3Map()
    {
        map.setSize(3, 3);
        map.startGame();
    }
    public void get4x4Map()
    {
        map.setSize(4, 4);
        map.startGame();
    }
}
