using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivSpawner : MonoBehaviour {

    public Civilian SpawnedCiv;
    public float SpawnInterval = 5f;
    public int MaxSpawnsAlife = 10;

    private float _spawnCountTime = 0;
    private int _spawnsAlife = 0;

	// Use this for initialization
	void Start () {
        _spawnCountTime = 0;
        _spawnsAlife = 0;

    }
	
	// Update is called once per frame
	void Update () {
        _spawnCountTime += Time.deltaTime;

        if (_spawnCountTime > SpawnInterval)
        {
            SpawnCritter();

            _spawnCountTime = 0;

            var children = GetComponentsInChildren<Civilian>();
            _spawnsAlife = children.Length;
        }
    }

    private void SpawnCritter()
    {
        if (_spawnsAlife >= MaxSpawnsAlife)
        {
            return;
        }

        var child = Instantiate(SpawnedCiv, transform.position, transform.rotation);
        child.transform.parent = gameObject.transform;
    }
}
