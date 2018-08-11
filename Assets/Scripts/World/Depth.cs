using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour {


    public bool update = false;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    // Update is called once per frame
    void Update () {

        if (update)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }

	}
}
