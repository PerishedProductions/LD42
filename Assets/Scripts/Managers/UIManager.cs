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

	// Use this for initialization
	void Start () {
		
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

	}
}
