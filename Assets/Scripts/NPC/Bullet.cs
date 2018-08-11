using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var civilian = collision.gameObject.GetComponent<Civilian>();

        if(civilian != null)
        {
            civilian.IsDieing = true;
        }
        else if (collision.tag == "Player")
        {
            GameManager.instance.PlayerReset();
        }

        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
