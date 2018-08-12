using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : Civilian {

    protected Civilian _target;
    public Weapon Weapon;
    public float TargetingTimer = 1f;
    public float ContinueMurderingChance = 0.33f;
    public float ChaseTargetChance = 0.9f;

    protected override void Start()
    {
        Weapon.Start();

        base.Start();
    }

    protected override void FixedUpdate()
    {
        Weapon.Update();

        base.FixedUpdate();
    }

    protected override void Idle()
    {
        base.Idle();
    }

    protected override void Attack()
    {
        switch (State)
        {
            case NpcPhysicalState.Moving:
                {
                    if (_moveDirection == Vector2.zero)
                    {
                        AdjustMovementToTarget();
                        _waitedFor = 0;
                        _waitingTime = Random.Range(0.1f, 1f);
                    }

                    MoveToDirection();

                    _waitedFor += Time.deltaTime;

                    if (_waitedFor > _waitingTime)
                    {
                        StopMoving();
                        State = NpcPhysicalState.Attacking;
                    }
                }
                break;
            case NpcPhysicalState.Targeting:
                {
                    StopMoving();

                    var targets = Physics2D.OverlapCircleAll(_rigidBody.position, Weapon.Range);

                    var targetIndex = Random.Range(0, targets.Length);

                    _target = targets[targetIndex].gameObject.GetComponentInChildren<Civilian>();

                    if (_target != null && _target != this)
                    {
                        _moveDirection = Vector2.zero;
                        State = NpcPhysicalState.Attacking;
                    }
                    else
                    {
                        _waitedFor += Time.deltaTime;

                        if (_waitedFor > _waitingTime)
                        {
                            _moveDirection = Vector2.zero;
                            EmotionalState = NpcEmotion.Idle;
                            State = NpcPhysicalState.Waiting;

                            _waitedFor = 0;
                        }
                    }

                    break;
                }
            case NpcPhysicalState.Attacking:
                {
                    if (Weapon.WeaponState == Weapon.WeaponStates.NeedsReloading)
                    {
                        Weapon.Reload();
                        return;
                    }

                    if (Weapon.WeaponState == Weapon.WeaponStates.IsReloading || Weapon.WeaponState == Weapon.WeaponStates.IsInCoolDown)
                    {
                        if (Weapon.WeaponState != Weapon.WeaponStates.IsReadyToShoot)
                        {
                            return;
                        }
                    }

                    if (_target == null || _target.transform == null || (_target != null && _target.IsDieing))
                    {
                        if (Random.value < ContinueMurderingChance)
                        {
                            State = NpcPhysicalState.Targeting;
                        }
                        else
                        {
                            EmotionalState = NpcEmotion.Idle;
                            State = NpcPhysicalState.Waiting;
                        }

                        _target = null;
                        return;
                    }

                    if (Weapon.IsTargetInRange(_target.transform.position, transform.position))
                    {
                        var direction = _target.transform.position - transform.position;
                        direction.Normalize();

                        Weapon.ShootAtTarget(transform.position + new Vector3(direction.x * 2, direction.y * 2, direction.z * 2), direction);

                        _target.TargetedBy(this);
                    }
                    else
                    {
                        //Move closer to target or chance to give up on target and move on
                        if (Random.value < ChaseTargetChance)
                        {
                            State = NpcPhysicalState.Moving;
                            return;
                        }

                        EmotionalState = NpcEmotion.Idle;
                        State = NpcPhysicalState.Waiting;
                        _target = null;

                    }

                    break;
                }
            case NpcPhysicalState.Waiting:
            case NpcPhysicalState.Defending:
            default:
                State = NpcPhysicalState.Targeting;
                break;
        }
    }

    public override void TargetedBy(Civilian shooter)
    {
        if(_target == null || _target != shooter)
        {
            _target = shooter;

            EmotionalState = NpcEmotion.Aggresive;
            State = NpcPhysicalState.Attacking;
        }
    }

    protected void AdjustMovementToTarget()
    {
        var degrees = Vector2.Angle(transform.position, _target.transform.position);

        _moveDirection = DegreeToVector2(degrees);
    }
}
