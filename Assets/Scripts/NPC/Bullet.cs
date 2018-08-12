using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float despawnTime = 10;
    private float currDespawnTime = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var civilian = collision.gameObject.GetComponent<Civilian>();

        if(civilian != null)
        {
            civilian.DoDmg();
        }
        else if (collision.tag == "Player")
        {
            GameManager.instance.PlayerReset();
        }

        SimplePool.Despawn(gameObject);
    }
	
	// Update is called once per frame
	void Update () {

        currDespawnTime += Time.deltaTime;

        if (currDespawnTime >= despawnTime)
        {
            SimplePool.Despawn(gameObject);
        }
    }
}
