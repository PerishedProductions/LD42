using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameState { Win, Loose, Playing }

    public enum Sounds { Slurp, Swing }

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
    public int ghostCap = 10;

    public GameState gameState = GameState.Playing;

    public AudioSource slurp;
    public AudioSource swing;

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
            ChangeGameState(GameState.Win);   
        }

        if (playerDead)
        {
            currentPlayerCooldown -= Time.deltaTime;

            if (currentPlayerCooldown <= 0)
            {
                player.SetActive(true);
                playerDead = false;
                playerController.health = 100;


                int dmg = UnityEngine.Random.Range(0, 4);

                if (souls > dmg)
                {
                    souls -= dmg;
                }
                
                currentPlayerCooldown = playerCooldown;
                player.transform.position = playerSpawn.position;
            }

        }

        amountOfGhosts = FindObjectsOfType<Ghost>().Length;

        if (amountOfGhosts >= 10)
        {
            ChangeGameState(GameState.Loose);
        }

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

    public void ChangeGameState(GameState type)
    {

        gameState = type;

        switch (gameState)
        {
            case GameState.Win:
                Time.timeScale = 0;
                break;
            case GameState.Loose:
                Time.timeScale = 0;
                break;
        }

    }

    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.Slurp:
                slurp.Play();
                break;
            case Sounds.Swing:
                swing.Play();
                break;
        }
    }

}
