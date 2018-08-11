using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : Civilian {

    protected Civilian _target;

    public GameObject WeaponPrefab;
    public float ReloadTime;

    private float _currentReloadTime;

    public Weapon Weapon;

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
            case NpcPhysicalState.Waiting:
                break;
            case NpcPhysicalState.Moving:
                break;
            case NpcPhysicalState.Targeting:
                {
                    var targets = Physics2D.OverlapCircleAll(_rigidBody.position, Weapon.Range);

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
                    if(Weapon.WeaponState == Weapon.WeaponStates.NeedsReloading)
                    {
                        Weapon.Reload();
                        return;
                    }

                    if (Weapon.WeaponState == Weapon.WeaponStates.IsReloading || Weapon.WeaponState == Weapon.WeaponStates.IsInCoolDown)
                    {
                        if (_waitingTime < 0)
                        {
                            _waitingTime = Random.Range(MinWaitTime, MaxWaitTime);
                        }

                        _waitedFor += Time.deltaTime;

                        if (_waitedFor > _waitingTime)
                        {
                            _waitedFor = 0;
                            _waitingTime = -1;
                        }
                        return;
                    }

                    if (_target == null || _target.transform == null || (_target != null && _target.IsDieing))
                    {
                        EmotionalState = NpcEmotion.Idle;
                        State = NpcPhysicalState.Waiting;
                        _target = null;
                        return;
                    }

                    if (Weapon.IsTargetInRange(_target.transform.position, transform.position))
                    {
                        var direction = _target.transform.position - transform.position;
                        direction.Normalize();

                        Weapon.ShootAtTarget(new Vector3(direction.x * 3, direction.y * 3, direction.z * 3), direction);

                        _target.TargetedBy(this);
                    }
                    else
                    {
                        //Move closer to target or chance to give up on target and move on
                        EmotionalState = NpcEmotion.Idle;
                        State = NpcPhysicalState.Waiting;
                        _target = null;
                    }

                    break;
                }
            case NpcPhysicalState.Defending:
                break;
            default:
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
}
