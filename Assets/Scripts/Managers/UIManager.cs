using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI soulText;
    public TextMeshProUGUI timeText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        soulText.text = "Souls: " + GameManager.instance.souls;
        timeText.text = GameManager.instance.GetTimeLeft();
	}
}
