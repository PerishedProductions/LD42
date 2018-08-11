using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : Civilian {

    protected Civilian _target;

    public GameObject BulletPrefab;
    public float ReloadTime;

    private float _currentReloadTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Idle()
    {
        base.Idle();
    }

    protected override void Attack()
    {
        switch (State)
        {
            case NpcPhysicalState.Waiting:
                break;
            case NpcPhysicalState.Moving:
                break;
            case NpcPhysicalState.Targeting:
                {
                    var targets = Physics2D.OverlapCircleAll(_rigidBody.position, 5);

                    var targetIndex = Random.Range(0, targets.Length);

                    _target = targets[targetIndex].gameObject.GetComponentInChildren<Civilian>();

                    if(_target != null && _target != this)
                    {
                        State = NpcPhysicalState.Attacking;
                    }

                    break;
                }
            case NpcPhysicalState.Attacking:
                {
                    if (_waitingTime < 0)
                    {
                        _waitingTime = Random.Range(MinWaitTime, MaxWaitTime);
                    }

                    _waitedFor += Time.deltaTime;

                    if (_waitedFor > _waitingTime)
                    {
                        if (_target != null && _target.IsDieing)
                        {
                            EmotionalState = NpcEmotion.Idle;
                            State = NpcPhysicalState.Waiting;
                            _target = null;
                        }

                        var direction = _target.transform.position - transform.position;
                        direction.Normalize();

                        direction = new Vector3(direction.x * 3, direction.y * 3, direction.z * 3);

                        var newBullet = Instantiate(BulletPrefab, transform.position + direction, transform.rotation);
                        var bulletBody = newBullet.GetComponent<Rigidbody2D>();


                        bulletBody.velocity = direction;
                        _waitedFor = 0;
                        _waitingTime = -1;
                    }

                    break;
                }
            case NpcPhysicalState.Defending:
                break;
            default:
                break;
        }
    }
}
