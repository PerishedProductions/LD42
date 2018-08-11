using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour {

    public int MovementSpeed = 250;
    public float MinMovementTime = 1;
    public float MaxMovementTime = 3;
    public float MinWaitTime = 3;
    public float MaxWaitTime = 10;
    public Vector2 _randomDirection;
    private Rigidbody2D _rigidBody { get; set; }

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        PickDirection();
    }

    // Update is called once per frame
    void FixedUpdate()
    { 

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void PickDirection()
    {
        _randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        StartCoroutine(MoveToDirection());
    }

    IEnumerator MoveToDirection()
    {
        _rigidBody.velocity = _randomDirection * MovementSpeed * Time.deltaTime;

        var waitingTime = Random.Range(MinMovementTime, MaxMovementTime);
        float i = 0;
        while(i < waitingTime)
        {
            i += Time.deltaTime;
            yield return null;
        }

        _rigidBody.velocity = new Vector2(0,0);

        StartCoroutine(WaitForSomeTime());
    }

    IEnumerator WaitForSomeTime()
    {
        yield return new WaitForSeconds(Random.Range(MinWaitTime, MaxWaitTime));
        PickDirection();
    }
}
