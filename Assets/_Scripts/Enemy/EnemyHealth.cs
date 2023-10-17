using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyHealth : AbstractHealth
{
    [SerializeField] private ChainIKConstraint _damagePunch;
    [SerializeField] private Transform _head;
    [SerializeField] private float _aimPunch;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private CharacterController2D _characterController;
    [SerializeField] private Collider2D _collider2D;

    public override void Damage(float damage, int rigidbody, Vector3 direction, Vector3 point)
    {
        _damagePunch.transform.position = _head.position + direction * _aimPunch;
        _damagePunch.transform.rotation = _head.rotation;
        base.Damage(damage,rigidbody,direction,point);
    }

    public override void Dead(float damage, int rigidbody, Vector3 direction)
    {
        _collider2D.enabled = false;
        _characterController.enabled = false;
        EnemyManager.instanse.deadEnemy.Invoke(GetComponent<EnemyStateMachineController>());
        _skinnedMeshRenderer.materials[0].SetColor("_FlatRimColor", Color.black);
        base.Dead(damage, rigidbody, direction);
    }
}
