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
    protected float _waitingTime;
    protected float _waitedFor;

    public int MovementSpeed = 250;
    public float MinMovementTime = 1;
    public float MaxMovementTime = 3;
    protected float MinWaitTime = 3;
    protected float MaxWaitTime = 10;
    public float DeathTimer = 100f;
    public NpcEmotion EmotionalState = NpcEmotion.Idle;
    public NpcPhysicalState State = NpcPhysicalState.Waiting;
    public GameObject soulPrefab;

    protected Vector2 _moveDirection;
    protected Rigidbody2D _rigidBody;

    // Use this for initialization
    protected virtual void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    protected virtual void FixedUpdate()
    {
        if(IsDieing)
        {
            CountDeath();
        }


        switch (EmotionalState)
        {
            case NpcEmotion.Death:
                {
                    Instantiate(soulPrefab, transform.position, transform.rotation);
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
                    StopMoving();

                    if (_moveDirection != Vector2.zero)
                    {
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

    public virtual void TargetedBy( Civilian shooter )
    {
        State = NpcPhysicalState.Moving;
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

    public Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    protected void PickRandomMovementDirection()
    {
        var degrees = Vector2.Angle(transform.position, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));

        _moveDirection = DegreeToVector2(degrees);
    }

    protected void MoveToDirection()
    {
        _rigidBody.velocity = _moveDirection * MovementSpeed * Time.deltaTime;
    }

    protected void StopMoving()
    {
        _rigidBody.velocity = new Vector2(0, 0);
        _moveDirection = Vector2.zero;
    }
}
