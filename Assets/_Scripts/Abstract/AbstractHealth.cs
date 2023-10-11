using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Serialization;

public class AbstractHealth : MonoBehaviour
{
    [SerializeField] protected float _health;
    [SerializeField] protected ParticleSystem _particleSystem;
    [SerializeField] protected float _damageForce;
    [SerializeField] protected List<Rigidbody> _rbs = new();
    [SerializeField] private List<RagdollData> _rigidbodyDatasTemp = new();
    protected float _currentHealth;
    protected Animator _animator;
    protected Transform _particleTransform;
    private Transform[] _goChild;

    private List<CharacterJoint> _characterJointsTemp = new();

    public virtual void Start()
    {
        _goChild = GetComponentsInChildren<Transform>();
        _currentHealth = _health;
        _rbs = GetComponentsInChildren<Rigidbody>().ToList();
        _particleTransform = _particleSystem.transform;
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
        
        for (int i=0; i < _rbs.Count; i++)
        {
            _rbs[i].transform.gameObject.AddComponent<DamageZone>().SetHealthController(this, i);
            var zev = new RagdollData();
            zev.transformRB = _rbs[i].transform;
            zev.rigidbodyData = GetDataRigidbody(_rbs[i]);
            if (_rbs[i].transform.gameObject.TryGetComponent<CharacterJoint>(out var joint))
            {
                zev.joint = GetJointData(joint);
                _characterJointsTemp.Add(joint);
            }
            _rigidbodyDatasTemp.Add(zev);
        }

        foreach (var joint in _characterJointsTemp)
        {
            Destroy(joint);
        }

        foreach (var rigid in _rbs)
        {
            Destroy(rigid);
        }
        _characterJointsTemp.Clear();
        _rbs.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dead(10, 1, Vector3.forward);
        }
    }

    public virtual void Damage(float damage, int rigidbody, Vector3 direction, Vector3 point)
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

    public virtual void Dead(float damage, int rigidbody, Vector3 direction)
    {
        _animator.enabled = false;
        AddRagDoll();
        _rigidbodyDatasTemp[rigidbody].rigidbodyData.rigidBody.AddForce(direction.normalized*_damageForce*damage, ForceMode.Impulse);
        StartCoroutine(SetStatic());
    }

    private void AddRagDoll()
    {
        for(int i=0; i< _rigidbodyDatasTemp.Count;i++)
        {
            Rigidbody rigid = _rigidbodyDatasTemp[i].transformRB.gameObject.AddComponent<Rigidbody>();
            Debug.Log(_rigidbodyDatasTemp[i].transformRB + i.ToString());
            _rigidbodyDatasTemp[i].rigidbodyData.rigidBody = rigid;
            rigid.mass = _rigidbodyDatasTemp[i].rigidbodyData.mass;
            rigid.angularDrag = _rigidbodyDatasTemp[i].rigidbodyData.angularDrag;
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.detectCollisions = true;
          
            if (_rigidbodyDatasTemp[i].joint !=null && _rigidbodyDatasTemp[i].joint.needJoint)
            {
                var joi = _rigidbodyDatasTemp[i].transformRB.gameObject.AddComponent<CharacterJoint>();
                _rigidbodyDatasTemp[i].joint.characterJoint = joi;
                joi.connectedBody = _rigidbodyDatasTemp[i].joint.connectedBody.GetComponent<Rigidbody>();
                joi.axis = _rigidbodyDatasTemp[i].joint.axis;
                joi.connectedAnchor =_rigidbodyDatasTemp[i].joint.connectedAncor; 
                joi.swingAxis = _rigidbodyDatasTemp[i].joint.swingAxis;
                SoftJointLimit limit = default;
                limit.limit = _rigidbodyDatasTemp[i].joint.lowTwistLimitSpring;
                joi.lowTwistLimit = limit;
                limit.limit = _rigidbodyDatasTemp[i].joint.hightTwistLimitSpring;
                joi.highTwistLimit = limit;
                limit.limit = _rigidbodyDatasTemp[i].joint.swingOneLimitLimitSpring;
                joi.swing1Limit = limit;
            }
        }
    }

    public virtual IEnumerator SetStatic()
    {
        yield return new WaitForSeconds(5f);
        foreach (var rb in _rigidbodyDatasTemp)
        {
            if (rb.joint!=null)
              Destroy(rb.joint.characterJoint);
            Destroy(rb.rigidbodyData.rigidBody);
        }
        foreach (var go in _goChild)
        {
            go.gameObject.isStatic = true;
        }
        gameObject.isStatic = true;
        _rigidbodyDatasTemp.Clear();
    }

    private RigidbodyData GetDataRigidbody(Rigidbody rb)
    {
        var rbd = new RigidbodyData();
        rbd.mass = rb.mass;
        rbd.angularDrag = rb.angularDrag;
        return rbd;
    }

   

    private JointData GetJointData(CharacterJoint joint)
    {
        var data = new JointData();
        data.needJoint = true;
        data.connectedBody = joint.connectedBody.transform;
        data.axis = joint.axis;
        data.connectedAncor = joint.connectedAnchor;
        data.swingAxis = joint.swingAxis;
        data.lowTwistLimitSpring = joint.lowTwistLimit.limit;
        data.hightTwistLimitSpring = joint.highTwistLimit.limit;
        data.swingOneLimitLimitSpring = joint.swing1Limit.limit;
        return data;
    }
    
    [Serializable]
    private class RagdollData
    {
        public Transform transformRB ;
        public JointData joint;
        public RigidbodyData rigidbodyData;
    }
    [Serializable]
    private class RigidbodyData
    {
        public Rigidbody rigidBody;
        public float mass;
        public float angularDrag;
    }
    [Serializable]
    private class JointData
    {
        public bool needJoint = false;
        [FormerlySerializedAs("_characterJoint")] public CharacterJoint characterJoint;
        public Transform connectedBody;
        public Vector3 axis, connectedAncor,swingAxis;
        public float swingOneLimitLimitSpring, lowTwistLimitSpring, hightTwistLimitSpring;
    }
}
