using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button restartButton;
    public Button ThreeButton;
    public Button FourButton;
    private Map map;
    public Text coin;
    private void Awake()
    {
        map = GetComponent<Map>();
    }
    private void Update()
    {
        coinText();
    }
    void coinText()
    {
        coin.text = "COIN " + GameManager.Instance.getCoinAmount().ToString();
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
