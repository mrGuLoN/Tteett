using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed, _damage;
    [SerializeField] private LayerMask _layerMask;

    private Vector3 _previousePosition, _direction;
    private Transform _thisTR;
    private float _distance, _timer = 3;

    private void Start()
    {
        _thisTR = GetComponent<Transform>();
        _previousePosition = _thisTR.position;
    }

    public void UpdateData(float speed, float damage, LayerMask layerMask)
    {
        _speed = speed;
        _damage = damage;
        _layerMask= layerMask;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer -= Time.deltaTime;
        _thisTR.position += _thisTR.forward * _speed * Time.deltaTime;
        _distance = Vector3.Distance(_previousePosition, _thisTR.position);
        if (Physics.Raycast(_previousePosition, _thisTR.forward, out var hit, _distance, _layerMask))
        {
            if (hit.transform.TryGetComponent<DamageZone>(out var damageZOne))
            {
                damageZOne.Damage(_damage, _thisTR.forward,hit.point);
            }
            Destroy(this.gameObject);
        }

        if (_timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
