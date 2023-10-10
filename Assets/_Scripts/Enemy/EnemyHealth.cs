using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class EnemyHealth : AbstractHealth
{
    [SerializeField] private ChainIKConstraint _damagePunch;
    [SerializeField] private Transform _head;
    [SerializeField] private float _aimPunch;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int izi = Random.Range(0, _rbs.Length);
            Damage(1, _rbs[izi], new Vector3(Random.Range(-1, 1),0,Random.Range(-1, 1)), _rbs[izi].transform.position);
        }
    }

    public override void Damage(float damage, Rigidbody rigidbody, Vector3 direction, Vector3 point)
    {
        _damagePunch.transform.position = _head.position + direction * _aimPunch;
        _damagePunch.transform.rotation = _head.rotation;
        base.Damage(damage,rigidbody,direction,point);
    }
    
}
