using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyHealth : AbstractHealth
{
    [SerializeField] private ChainIKConstraint _damagePunch;
    [SerializeField] private Transform _head;
    [SerializeField] private float _aimPunch;
   

    public override void Damage(float damage, int rigidbody, Vector3 direction, Vector3 point)
    {
        _damagePunch.transform.position = _head.position + direction * _aimPunch;
        _damagePunch.transform.rotation = _head.rotation;
        base.Damage(damage,rigidbody,direction,point);
    }
    
}
