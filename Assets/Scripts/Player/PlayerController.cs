using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int movementSpeed = 500;
    public float attackTime = 1;

    public int health = 100;

    public Collider2D hitbox;
    public Vector2[] hitboxPos;

    private Rigidbody2D rb;
    private Animator anim;

    private bool attack = false;
    private bool canAttack = true;
    private float currAttackTime = 0;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

	}

    private void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);

        if (movement.x != 0 || movement.y != 0)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            attack = true;
            canAttack = false;
        }

        if (movement.x < 0)
        {
            hitbox.offset = hitboxPos[0];
        }
        else if (movement.x > 0)
        {
            hitbox.offset = hitboxPos[1];
        }

        if (movement.y < 0)
        {
            hitbox.offset = hitboxPos[3];
        }
        else if (movement.y > 0)
        {
            hitbox.offset = hitboxPos[2];
        }

        if (attack)
        {

            hitbox.enabled = true;

            currAttackTime += Time.deltaTime;

            if (currAttackTime >= attackTime)
            {
                canAttack = true;
                attack = false;
                currAttackTime = 0;
            }
        }
        else
        {
            hitbox.enabled = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate () {

		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.velocity = movement * movementSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            GameManager.instance.AddSoul();
            Destroy(collision.gameObject);
        }


        canAttack = true;
        attack = false;
    }
}
