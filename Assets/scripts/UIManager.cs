using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    public Slider rowScroll;
    public Slider colScroll;

    public GameObject answerMap;
    public GameObject blocks;
    private bool isAnswerOpen = false;
    public Text answerText;
    public GameObject guidePanel;
    private bool isguideOpen = false;
    public GameObject stopGamePanel;
    private bool sound = true;
    public Image soundImage;
    public Sprite soundOn;
    public Sprite soundOff;

    private void Awake()
    {
        map = GetComponent<Map>();
    }
    private void Start()
    {
        answerMap = GameObject.Find("BlocksAnswer");
        blocks = GameObject.Find("Blocks");
    }
    private void Update()
    {

        row.text = "ROW : " + rowScroll.value;
        column.text = "COLUMN : " + colScroll.value;
        if (answerMap == null)
            answerMap = map.getAnswerParentObject();
        if(blocks == null)
            blocks = GameObject.Find("Blocks");
            
        if (isAnswerOpen) {
             answerMap.SetActive(true); 
             answerText.text = "CLOSE"; 
             blocks.SetActive(false);
        }
        else { 
            answerMap.SetActive(false); 
            answerText.text = "ANSWER"; 
            blocks.SetActive(true);
        }

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

        if (isguideOpen) { guidePanel.SetActive(true); }
        else { guidePanel.SetActive(false); }
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
        int r = (int)rowScroll.value;
        int c = (int)colScroll.value;

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
    public void openGuidePanel()
    {
        SoundManager.Instance.playSfx("Button");
        isguideOpen = isguideOpen != true;
    }

    public void stopGame()
    {
        SoundManager.Instance.playSfx("Button");
        Time.timeScale = 0;
        stopGamePanel.SetActive(true);
    }
    public void continueGame()
    {
        SoundManager.Instance.playSfx("Button");
        Time.timeScale = 1;
        stopGamePanel.SetActive(false);
    }
    public void openInstruction()
    {
        SoundManager.Instance.playSfx("Button");
        instractionPanel.SetActive(true);
    }
    public void soundOptions()
    {
        SoundManager.Instance.playSfx("Button");
        sound = sound != true;
        if (sound)
        {
            SoundManager.Instance.soundOn();
            soundImage.sprite = soundOn;
        }
        else
        {
            SoundManager.Instance.soundOff();
            soundImage.sprite = soundOff;
        }
    }

}
