using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum EnemyState { Idle, Defensive, Aggressive }

    public EnemyState state = EnemyState.Idle;
    public int moveSpeed = 1;
    public int viewRange = 5;

    public Rigidbody2D rb;

	// Use this for initialization
	public virtual void Start () {

        rb = GetComponent<Rigidbody2D>();

        int rand = UnityEngine.Random.Range(0, 2);

        switch (rand)
        {
            case 0:
                state = EnemyState.Idle;
                break;
            case 1:
                state = EnemyState.Defensive;
                break;
            case 2:
                state = EnemyState.Aggressive;
                break;
        }

    }
	
	// Update is called once per frame
	public virtual void Update ()
    {

        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Defensive:
                Defensive();
                break;
            case EnemyState.Aggressive:
                Aggressive();
                break;
        }

    }

    public virtual void Idle()
    {

    }

    public virtual void Defensive()
    {
        var targets = Physics2D.OverlapCircleAll(rb.position, viewRange);

        GameObject target = null;

        if (targets != null)
        {

            bool playerFound = false;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].tag == "Player")
                {
                    target = targets[i].gameObject;
                    playerFound = true;
                }

            }

            if (target != null && playerFound == false)
            {
                target = null;
            }

        }

        if (target != null)
        {
            var direction = target.transform.position - transform.position;
            rb.velocity = -Vector2.Lerp(Vector2.zero, direction, 0.5f) * moveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public virtual void Aggressive()
    {

    }

}
