using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum EnemyState { Idle, Defensive, Aggressive }

    public EnemyState state = EnemyState.Idle;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();

        int rand = Random.Range(0, 2);

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
	void Update ()
    {

        switch (state)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Defensive:

                var targets = Physics2D.OverlapCircleAll(rb.position, 5);

                if (targets != null)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (targets[i].tag == "Player")
                        {
                            rb.velocity = targets[i].GetComponent<Rigidbody2D>().velocity / 2;
                        }
                    }
                }

                break;
            case EnemyState.Aggressive:
                break;
        }

    }
}
