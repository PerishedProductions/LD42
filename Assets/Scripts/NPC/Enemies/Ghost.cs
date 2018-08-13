using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy {

    public override void Start()
    {
        base.Start();

        state = EnemyState.Aggressive;

    }

    public override void Aggressive()
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
            rb.velocity = Vector2.Lerp(Vector2.zero, direction, 0.5f) * moveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        base.Aggressive();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.HurtPlayer();

            int rand = Random.Range(0, 3);

            if (rand > 0)
            {
                GameManager.instance.souls -= rand;
            }
            else
            {
                GameManager.instance.souls = 0;
            }
        }
    }

}
