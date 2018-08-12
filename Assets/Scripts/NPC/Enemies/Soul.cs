using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : Enemy {

    public float lifeTime = 5;
    public float currLifeTime = 0;

    public GameObject ghostPrefab;

    private Animator anim;

    public override void Start()
    {
        base.Start();

        anim = GetComponentInChildren<Animator>();

        Color randColor = new Color32(
             (byte)Random.Range(0, 255),
             (byte)Random.Range(0, 255),
             (byte)Random.Range(0, 255),
             255);

        GetComponentInChildren<SpriteRenderer>().color = randColor;

    }

    public override void Update()
    {

        anim.SetFloat("Horizontal", rb.velocity.x);
        anim.SetFloat("Vertical", rb.velocity.y);

        currLifeTime += Time.deltaTime;

        if (currLifeTime >= lifeTime)
        {
            Instantiate(ghostPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        base.Update();
    }

}
