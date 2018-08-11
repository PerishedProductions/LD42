﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum WeaponStates
    {
        NeedsReloading,
        IsReloading,
        IsReadyToShoot,
        IsInCoolDown
    }

    public GameObject BulletPrefab;

    public float Reloadtime = 1f;
    public int MagazineSize = 1;
    public float Range = 5;
    public float ShootingSpeed = 0.1f;
    protected int _ammoLeft = 1;
    protected float _currentReloadtime = 0;
    protected float _cdTime = 0;

    public WeaponStates WeaponState = WeaponStates.IsReadyToShoot;


	// Use this for initialization
	public void Start () {
        _ammoLeft = MagazineSize;
        WeaponState = WeaponStates.IsReadyToShoot;
	}
	
	// Update is called once per frame
	public void Update () {
        switch (WeaponState)
        {
            case WeaponStates.IsInCoolDown:
                {
                    if(_cdTime < 0)
                    {
                        _cdTime = 0;
                    }

                    _cdTime += Time.deltaTime;

                    if(_cdTime > ShootingSpeed)
                    {
                        WeaponState = WeaponStates.IsReadyToShoot;
                        _cdTime = 0;
                    }

                    break;
                }
            case WeaponStates.IsReloading:
                {
                    if (_currentReloadtime < 0)
                    {
                        _currentReloadtime = 0;
                    }

                    _currentReloadtime += Time.deltaTime;

                    if (_currentReloadtime > Reloadtime)
                    {
                        _ammoLeft = MagazineSize;
                        WeaponState = WeaponStates.IsReadyToShoot;
                        _currentReloadtime = 0;
                    }
                    break;
                }
            case WeaponStates.NeedsReloading:
            case WeaponStates.IsReadyToShoot:
            default:
                break;
        }
	}

    public virtual bool IsTargetInRange(Vector2 position, Vector2 targetPosition)
    {
        var distance = Vector3.Distance(position, targetPosition);
        return distance < Range;
    }

    public virtual void Reload()
    {
        WeaponState = WeaponStates.IsReloading;
    }

    public virtual void ShootAtTarget(Vector3 offset, Vector3 direction)
    {
        if(WeaponState == WeaponStates.IsReadyToShoot)
        {
            var newBullet = Instantiate(BulletPrefab, transform.position + offset, transform.rotation);
            var bulletBody = newBullet.GetComponent<Rigidbody2D>();

            bulletBody.velocity = direction;

            _ammoLeft--;

            WeaponState = WeaponStates.IsInCoolDown;

            if(_ammoLeft < 0)
            {
                WeaponState = WeaponStates.NeedsReloading;
            }
        }
    }
}