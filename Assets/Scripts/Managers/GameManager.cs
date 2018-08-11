using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int souls = 0;
    public float timeLeft;
    public float timeLimit = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        timeLeft = timeLimit;

    }

    // Update is called once per frame
    void Update () {

        timeLeft -= Time.deltaTime;

        if (timeLeft >= timeLimit)
        {
            Debug.Log("Times up");            
        }

	}

    public void AddSoul()
    {
        souls++;
    }

    public string GetTimeLeft()
    {
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);

        return string.Format("{0}:{1:00}",(int)time.TotalMinutes, time.Seconds);

    }

}
