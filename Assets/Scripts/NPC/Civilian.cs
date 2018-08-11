﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour {

    public enum NpcEmotion
    {
        Idle,
        Aggresive,
        Fear,
        Defensive,
        Death
    }

    public enum NpcPhysicalState
    {
        Waiting,
        Moving,
        Attacking,
        Defending,
        Targeting
    }

    public bool IsDieing = false;
    private float _waitingTime;
    private float _waitedFor;

    public int MovementSpeed = 250;
    public float MinMovementTime = 1;
    public float MaxMovementTime = 3;
    public float MinWaitTime = 3;
    public float MaxWaitTime = 10;
    public float DeathTimer = 100f;
    public NpcEmotion EmotionalState = NpcEmotion.Idle;
    public NpcPhysicalState State = NpcPhysicalState.Waiting;

    private Vector2 _moveDirection;
    protected Rigidbody2D _rigidBody { get; set; }

    // Use this for initialization
    protected virtual void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if(IsDieing)
        {
            CountDeath();
        }


        switch (EmotionalState)
        {
            case NpcEmotion.Death:
                {
                    Destroy(gameObject);
                    break;
                }
            case NpcEmotion.Aggresive:
                {
                    Attack();
                    break;
                }
            case NpcEmotion.Fear:
                {
                    break;
                }
            case NpcEmotion.Defensive:
                {
                    break;
                }
            case NpcEmotion.Idle:
            default:
                {
                    Idle();
                    break;
                }
        }
    }

    protected virtual void Idle()
    {
        switch (State)
        {
            case NpcPhysicalState.Waiting:
                {
                    if(_moveDirection != Vector2.zero)
                    {
                        StopMoving();
                        _moveDirection = Vector2.zero;
                        _waitedFor = 0;
                        _waitingTime = -1;
                    }

                    if(_waitingTime < 0)
                    {
                        _waitingTime = Random.Range(MinWaitTime, MaxWaitTime);
                    }

                    _waitedFor += Time.deltaTime;

                    if (_waitedFor > _waitingTime)
                    {
                        State = NpcPhysicalState.Moving;

                        _waitedFor = 0;
                    }

                    break;
                }

            case NpcPhysicalState.Moving:
                {
                    if(_moveDirection == Vector2.zero)
                    {
                        PickRandomMovementDirection();
                        _waitedFor = 0;
                        _waitingTime = Random.Range(MinMovementTime, MaxMovementTime);
                    }

                    MoveToDirection();

                    _waitedFor += Time.deltaTime;

                    if(_waitedFor > _waitingTime)
                    {
                        StopMoving();

                        State = NpcPhysicalState.Waiting;
                    }

                    break;
                }
            case NpcPhysicalState.Attacking:
            case NpcPhysicalState.Defending:
            default:
                break;
        }
    }

    protected virtual void CountDeath()
    {
        DeathTimer -= Time.deltaTime;

        if (DeathTimer < 0)
        {
            EmotionalState = NpcEmotion.Death;
        }
    }


    protected virtual void Hide()
    {

    }

    protected virtual void Run()
    {

    }

    protected virtual void Attack()
    {

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void PickRandomMovementDirection()
    {
        _moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    private void MoveToDirection()
    {
        _rigidBody.velocity = _moveDirection * MovementSpeed * Time.deltaTime;
    }

    private void StopMoving()
    {
        _rigidBody.velocity = new Vector2(0, 0);
    }
}
