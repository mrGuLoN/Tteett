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
            _direction = _enemyControllerSm.target.position  -
                         _enemyControllerSm.thisTransform.position;
            _distance = Vector3.Distance(_enemyControllerSm.thisTransform.position,
                _enemyControllerSm.target.position);
            if (Physics2D.Raycast(_enemyControllerSm.thisTransform.position, _direction,_distance,
                    _enemyControllerSm.layerWall))
            {
                _enemyControllerSm.thisTransform.up =  _direction.normalized;
                _enemyControllerSm.target = null;
                _enemyControllerSm.endPosition = _enemyControllerSm.thisTransform.position;
                _enemyControllerSm.ChangeState(_enemyControllerSm.enemyFindObject);
                return;
            }
            if (_distance <= _enemyControllerSm.attackDistance)
            {
                _enemyControllerSm.animator.SetBool("Hit", true);
                _enemyControllerSm.thisTransform.up =  _direction.normalized;
            }
            else
            {
                _enemyControllerSm.animator.SetBool("Hit", false);
                _enemyControllerSm.animator.SetBool("Fight", false);
                _enemyControllerSm.thisTransform.up = _direction.normalized;
                _enemyControllerSm.characterController.Move(_direction.normalized * (_enemyControllerSm.currentSpeed * Time.deltaTime));
            }
        }
    }
}