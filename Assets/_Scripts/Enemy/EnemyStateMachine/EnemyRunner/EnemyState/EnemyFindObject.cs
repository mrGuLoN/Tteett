using UnityEngine;

namespace Enemy.StateMachine.States
{
    public class EnemyFindObject: BaseEnemyState
    {
        private float _distance;
        private float _distanceCheck;
        private Vector3 _direction;
        private Vector3 _directionCheck;
        
        private EnemyStateMachineController _enemyControllerSm;
        
        public EnemyFindObject(EnemyStateMachineController stateMachine) : base("EnemyControllerSM", stateMachine)
        {
            _enemyControllerSm = (EnemyStateMachineController)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.animator.SetBool("Walk", true);
            _enemyControllerSm.animator.SetBool("Hit", false);
        }
        
        public override void UpdatePhysics()
        {
          
        }

        public override void UpdateMovement(Transform target)
        {
            _direction = (_enemyControllerSm.endPosition - _enemyControllerSm.thisTransform.position).normalized;
            _enemyControllerSm.thisTransform.forward = -1*new Vector3(_direction.x,0, _direction.z);
            _enemyControllerSm.characterController.Move(_direction * _enemyControllerSm.currentSpeed * Time.deltaTime+Vector3.down);
            _distance = Vector3.Distance(_enemyControllerSm.thisTransform.position, _enemyControllerSm.endPosition);
           
            if (_distance <= 1f)
            {
                _enemyControllerSm.target = null;
                _enemyControllerSm.ChangeState(_enemyControllerSm.enemyIdle);
                return;
            }
           
                _distanceCheck = Vector3.Distance(_enemyControllerSm.thisTransform.position,
                    target.position);
                if (_distanceCheck <= _enemyControllerSm.loocDistance)
                {
                    _directionCheck = (target.position + Vector3.up) -
                                      (_enemyControllerSm.thisTransform.position + Vector3.up);
                    _enemyControllerSm.thisTransform.forward =  -1*new Vector3(_directionCheck.x,0, _directionCheck.z).normalized;
                    if (!Physics.Raycast(_enemyControllerSm.thisTransform.position + Vector3.up, _directionCheck, _distanceCheck,
                            _enemyControllerSm.layerWall))
                    {
                        _enemyControllerSm.target =target;
                        _enemyControllerSm.targetHealthController = target.GetComponent<AbstractHealth>();
                        _enemyControllerSm.ChangeState(_enemyControllerSm.enemyAttack);
                        return;
                    }
                }
            
        }
    }
}