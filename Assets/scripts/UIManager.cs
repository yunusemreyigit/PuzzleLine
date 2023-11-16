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

    public Text gameTimer;
    private float min = 0, sec = 0;

    public Text difficulty;

    float timer;


    public Text row;
    public Text column;

    public GameObject answerMap;
    private bool isAnswerOpen = false;
    public Text answerText;
    private void Awake()
    {
        map = GetComponent<Map>();
    }
    private void Start()
    {
        answerMap = GameObject.Find("BlocksAnswer");
    }
    private void Update()
    {

        if (answerMap == null)
            answerMap = map.getAnswerParentObject();

        if (isAnswerOpen) { answerMap.SetActive(true); answerText.text = "CLOSE"; }
        else { answerMap.SetActive(false); answerText.text = "SHOW"; }

        GameTimer();
        difficulty.text = map.gameDifficulty().ToString();

        if (map.isFinished())
        {
            timer = 0;
        }

        if (timer >= 1)
        {
            timer = 0;
        }
        else
        {
            levelControl();
        }

        int v = GameManager.Instance.getLevel();
        currLevel.text = v.ToString();
        nextLevel.text = (v + 1).ToString();
    }

    private void GameTimer()
    {
        sec = GameManager.Instance.getSecond();
        min = GameManager.Instance.getMinute();
        gameTimer.text = String.Format("{0:00}", min) + ":" + String.Format("{0:00}", sec);
    }

    private void levelControl()
    {
        timer += Time.deltaTime;
        float x = Mathf.Lerp(xpBar.localScale.x, GameManager.Instance.getXp() / GameManager.Instance.getRequiredXp(), animTime(timer));
        xpBar.localScale = new Vector3(x, .7f, 1);
        if (x >= 1)
        {
            SoundManager.Instance.playSfx("IncreaseLevel");
            xpBar.localScale = new(0, 1, 1);
            GameManager.Instance.addLevel();
        }
    }
    private float animTime(float x)
    {
        return x * x * x * x * x;
    }

    public void restartGame()
    {
        SoundManager.Instance.playSfx("Button");
        map.restartScene();
        GameManager.Instance.resetTimer();
    }
    public void getPrivatePanel()
    {
        privateGame.SetActive(true);
    }
    public void randomMap()
    {
        SoundManager.Instance.playSfx("Button");
        var x = UnityEngine.Random.Range(3, 7);
        var y = UnityEngine.Random.Range(3, 7);
        map.setSize(x, y);
        map.startGame();
        answerMap = map.getAnswerParentObject();
        GameManager.Instance.resetTimer();

    }
    public void exitInstraction()
    {
        instractionPanel.SetActive(false);
    }
    public void startPrivateGame()
    {
        int r = short.Parse(row.text);
        int c = short.Parse(column.text);
        if (r > 10 || c > 10 || r < 3 || c < 3) return;
        privateGame.SetActive(false);
        map.setSize(r, c);
        map.startGame();
        answerMap = map.getAnswerParentObject();
        GameManager.Instance.resetTimer();

    }
    public void privateGameExitPanel()
    {
        privateGame.SetActive(false);
    }
    public void openAnswer()
    {
        SoundManager.Instance.playSfx("Button");
        isAnswerOpen = isAnswerOpen == true ? false : true;
    }
}
