using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy.StateMachine.States
{
    [Serializable]
    public class EnemyAttack: BaseEnemyState
    {
        private RaycastHit _hit;
        private Vector3 _direction;
        private float _distance;
        private AbstractHealth _healthController;
        
        private EnemyStateMachineController _enemyControllerSm;
       

        public EnemyAttack(EnemyStateMachineController stateMachine) : base("EnemyAttack", stateMachine)
        {
            _enemyControllerSm = (EnemyStateMachineController)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.animator.SetBool("Walk", true);
            _healthController = _enemyControllerSm.target.GetComponent<AbstractHealth>();
        }

        public override void UpdateLogic()
        {
           
        }

        public override void UpdatePhysics()
        {
            _direction = (_enemyControllerSm.target.position + Vector3.up) -
                         (_enemyControllerSm.thisTransform.position + Vector3.up);
            _distance = Vector3.Distance(_enemyControllerSm.thisTransform.position,
                _enemyControllerSm.target.position);
            if (Physics.Raycast(_enemyControllerSm.thisTransform.position + Vector3.up, _direction,_distance,
                    _enemyControllerSm.layerWall))
            {
                _enemyControllerSm.thisTransform.forward =  new Vector3(_direction.x,0, _direction.z).normalized;
                _enemyControllerSm.target = null;
                _enemyControllerSm.endPosition = _enemyControllerSm.thisTransform.position;
                _enemyControllerSm.ChangeState(_enemyControllerSm.enemyFindObject);
                return;
            }
            if (_distance <= _enemyControllerSm.attackDistance)
            {
                _enemyControllerSm.animator.SetBool("Hit", true);
                _enemyControllerSm.thisTransform.forward =  -1*new Vector3(_direction.x,0, _direction.z).normalized;
            }
            else
            {
                _enemyControllerSm.animator.SetBool("Hit", false);
                _enemyControllerSm.animator.SetBool("Fight", false);
                _enemyControllerSm.thisTransform.forward =  -1*new Vector3(_direction.x,0, _direction.z).normalized;
                _enemyControllerSm.characterController.Move(_direction.normalized * (_enemyControllerSm.currentSpeed * Time.deltaTime)+Vector3.down);
            }
        }
    }
}