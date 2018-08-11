using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : Civilian {

    protected Civilian _target;

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

                    if(_target != null)
                    {
                        State = NpcPhysicalState.Attacking;
                    }

                    break;
                }
            case NpcPhysicalState.Attacking:
                {
                    if (_target != null)
                    {
                        _target.IsDieing = true;
                    }

                    EmotionalState = NpcEmotion.Idle;
                    State = NpcPhysicalState.Waiting;
                    break;
                }
            case NpcPhysicalState.Defending:
                break;
            default:
                break;
        }
    }
}
