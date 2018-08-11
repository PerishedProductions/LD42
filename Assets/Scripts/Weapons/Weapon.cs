using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public float Reloadtime = 1f;
    public int MagazineSize = 1;
    public float Range = 5;
    protected int _ammoLeft = 0;
    protected float _currentReloadtime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
