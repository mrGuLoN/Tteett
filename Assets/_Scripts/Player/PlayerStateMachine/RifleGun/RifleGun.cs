using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleGun : MonoBehaviour
{
    [SerializeField] private ParticleSystem _splash;
    [SerializeField] private BulletController _bullet;
    [SerializeField] private Transform _firePoint;

   

    public void Fire()
    {
        _splash.Play();
        var bullet = Instantiate(_bullet, _firePoint.position, _firePoint.rotation);
    }
}
