using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int souls = 0;
    public float timeLeft;
    public float timeLimit = 100;
    public Transform playerSpawn;

    public GameObject player;
    public PlayerController playerController;

    public float playerCooldown = 5;
    public float currentPlayerCooldown;
    public bool playerDead = false;

    public int amountOfGhosts = 0;

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
        currentPlayerCooldown = playerCooldown;

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update () {

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            Debug.Log("Times up");            
        }

        if (playerDead)
        {
            currentPlayerCooldown -= Time.deltaTime;

            if (currentPlayerCooldown <= 0)
            {
                player.SetActive(true);
                playerDead = false;
                playerController.health = 100;
                currentPlayerCooldown = 0;
                player.transform.position = playerSpawn.position;
            }

        }

        amountOfGhosts = GetComponents<Ghost>().Length;

        if (playerController.health <= 0)
        {
            PlayerReset();
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

    public void PlayerReset()
    {
        player.SetActive(false);
        playerDead = true;
    }

    public void HurtPlayer()
    {
        int hurtPercent = UnityEngine.Random.Range(0, 50);

        playerController.health -= hurtPercent;

    }

}
