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

    public RectTransform hpMask;
    float hpMaskLengthPercent;

    // Use this for initialization
    void Start () {
        hpMaskLengthPercent = hpMask.sizeDelta.x / 100;
    }
	
	// Update is called once per frame
	void Update () {
        soulText.text = "Souls: " + GameManager.instance.souls;
        timeText.text = GameManager.instance.GetTimeLeft();

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

	}
}
