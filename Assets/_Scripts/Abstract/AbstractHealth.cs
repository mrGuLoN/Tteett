using System.Collections;
using UnityEngine;

public class AbstractHealth : MonoBehaviour
{
    [SerializeField] protected float _health;
    [SerializeField] protected ParticleSystem _particleSystem;
    [SerializeField] protected float _damageForce;
    protected float _currentHealth;
    protected Rigidbody[] _rbs;
    protected Animator _animator;
    protected Transform _particleTransform;
    public virtual void Start()
    {
        _currentHealth = _health;
        _rbs = GetComponentsInChildren<Rigidbody>();
        _particleTransform = _particleSystem.transform;
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
        foreach (var rb in _rbs)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.transform.gameObject.AddComponent<DamageZone>().SetHealthController(this);
        }
    }

    public virtual void Damage(float damage, Rigidbody rigidbody, Vector3 direction, Vector3 point)
    {
        _currentHealth -= damage;
        _particleTransform.position = point;
        _particleTransform.forward = direction;
        _particleSystem.Play();
        if (_currentHealth > 0)
        {
            _animator.SetTrigger("Damage");
        }
        else
        {
            Dead(damage, rigidbody, direction);
        }
    }

    public virtual void Dead(float damage, Rigidbody rigidbody, Vector3 direction)
    {
        _animator.enabled = false;
        foreach (var rb in _rbs)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        rigidbody.AddForce(direction.normalized*_damageForce*damage, ForceMode.Impulse);
        StartCoroutine(SetStatic());
    }

    public virtual IEnumerator SetStatic()
    {
        yield return new WaitForSeconds(5f);
        foreach (var rb in _rbs)
        {
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.gameObject.isStatic = true;
        }
        gameObject.isStatic = true;
        GetComponentInChildren<SkinnedMeshRenderer>().gameObject.isStatic = true;
    }
}
