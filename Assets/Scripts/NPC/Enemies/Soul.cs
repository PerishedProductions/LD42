using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : Enemy {

    public float lifeTime = 5;
    public float currLifeTime = 0;

    public GameObject ghostPrefab;

    public override void Update()
    {

        currLifeTime += Time.deltaTime;

        if (currLifeTime >= lifeTime)
        {
            Instantiate(ghostPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        base.Update();
    }

}
