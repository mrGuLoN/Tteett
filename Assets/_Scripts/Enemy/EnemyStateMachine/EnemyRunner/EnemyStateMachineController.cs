using Enemy.StateMachine.States;
using UnityEngine;

public class EnemyStateMachineController : BaseEnemyStateMachine
{
    public Animator animator => _animator;
    public float loocDistance => _loocDistance;
    public float attackDistance => _attackDistance;
    public float walkSpeed => _walkSpeed;
    public float runSpeed => _runSpeed;
    public float currentSpeed => _currentSpeed;
    public Transform thisTransform => _thisTransform;
    public CharacterController2D characterController => _characterController;
    public Transform target;
    public AbstractHealth targetHealthController;
    public Vector3 endPosition;
    public LayerMask layerWall => _layerWall;
    
    public EnemyAttack enemyAttack;
    public EnemyIdle enemyIdle;
    public EnemyFindObject enemyFindObject;
    public BaseEnemyState currenState => _currentState;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private float _loocDistance, _attackDistance, _walkSpeed,_runSpeed, _currentSpeed;
    [SerializeField] protected float _damage;
    [SerializeField] private Transform _thisTransform;
    [SerializeField] protected LayerMask _layerWall;
    [SerializeField] private CharacterController2D _characterController;
    private bool _dead;

    public virtual void  Start()
    {
        enemyAttack = new EnemyAttack(this);
        enemyIdle = new EnemyIdle(this);
        enemyFindObject = new EnemyFindObject(this);
        EnemyManager.instanse.newEnemy.Invoke(this);
        ChangeState(enemyIdle);
        _currentSpeed = walkSpeed;
    }

    public virtual void UpdateSpeed(float constant)
    {
        _currentSpeed = walkSpeed + (runSpeed - walkSpeed) * constant;
    }


    public virtual void Punch()
    {
        if (target!=null && Vector3.Distance(thisTransform.position, target.position) < attackDistance)
        {
            targetHealthController.Damage(_damage,0,thisTransform.forward,target.position+Vector3.up*1.5f);
            if (_dead)
            {
                ChangeState(enemyIdle);
            }
        }
    }

    public virtual void DamageCheck(Vector3 direction)
    {
        if (_currentState != enemyAttack)
        {
             endPosition = direction;
             ChangeState(enemyFindObject);
        }
    }
}
