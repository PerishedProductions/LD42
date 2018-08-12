using System.Collections;
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

    public bool IsBleedingToDeath = false;
    protected float _waitingTime;
    protected float _waitedFor;

    public int MovementSpeed = 250;
    public float MinMovementTime = 1;
    public float MaxMovementTime = 3;
    protected float MinWaitTime = 3;
    protected float MaxWaitTime = 10;
    public NpcEmotion EmotionalState = NpcEmotion.Idle;
    public NpcPhysicalState State = NpcPhysicalState.Waiting;
    public GameObject soulPrefab;

    protected Vector2 _moveDirection;
    protected Rigidbody2D _rigidBody;

    public float Hitpoints = 100;

    public virtual void DoDmg()
    {
        Hitpoints -= 25;

        if (Hitpoints < 0) EmotionalState = NpcEmotion.Death;

        var criticalPain = Random.value;

        if(criticalPain < 0.2)
        {
            EmotionalState = NpcEmotion.Death;
        }
        else if(criticalPain < 0.5)
        {
            IsBleedingToDeath = true;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    protected virtual void FixedUpdate()
    {
        if(IsBleedingToDeath)
        {
            BleedDmg();
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

                    if(_waitingTime < 0)
                    {
                        _waitingTime = Random.Range(MinWaitTime, MaxWaitTime);
                    }

                    _waitedFor += Time.deltaTime;

                    if (_waitedFor > _waitingTime)
                    {
                        State = NpcPhysicalState.Moving;

                        _waitedFor = 0;
                        _waitingTime = -1;
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

                        _waitedFor = 0;
                        _waitingTime = -1;
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

    protected virtual void BleedDmg()
    {
        Hitpoints -= Time.deltaTime / 2;

        if (Hitpoints < 0)
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

    protected void PickRandomMovementDirection()
    {
        var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        _moveDirection = direction.normalized;
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
