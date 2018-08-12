using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI respawnText;
    public GameObject deadPanel;

    public TextMeshProUGUI ghostCount;

    public RectTransform hpMask;
    float hpMaskLengthPercent;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameOverScore;

    // Use this for initialization
    void Start () {
        hpMaskLengthPercent = hpMask.sizeDelta.x / 100;
    }
	
	// Update is called once per frame
	void Update () {
        soulText.text = "Souls: " + GameManager.instance.souls;
        timeText.text = GameManager.instance.GetTimeLeft();
        ghostCount.text = "Ghosts: " + GameManager.instance.amountOfGhosts;

        if (GameManager.instance.playerDead)
        {
            deadPanel.SetActive(true);

            respawnText.text = "Respawning in: " + (int)GameManager.instance.currentPlayerCooldown;

        }
        else
        {
            deadPanel.SetActive(false);
        }

        if (GameManager.instance.playerController.health >= 0)
        {
            hpMask.sizeDelta = new Vector2(hpMaskLengthPercent * GameManager.instance.playerController.health, hpMask.sizeDelta.y);
        }
        else
        {
            hpMask.sizeDelta = new Vector2(0, hpMask.sizeDelta.y);
        }

        if (GameManager.instance.gameState == GameManager.GameState.Win)
        {
            gameOverPanel.SetActive(true);
            gameOverScore.gameObject.SetActive(true);
            gameOverText.text = "Game Over";
            gameOverScore.text = "Souls Collected: " + GameManager.instance.souls + "\n " +
                "Ghosts Present: " + GameManager.instance.amountOfGhosts + "\n" +
                "Score Sum: " + (GameManager.instance.souls - GameManager.instance.amountOfGhosts);
        }

        if (GameManager.instance.gameState == GameManager.GameState.Loose)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "You lost!";
        }

    }
}
