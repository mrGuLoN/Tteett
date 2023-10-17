using System;
using UnityEngine;

namespace Enemy.StateMachine.States
{
    [Serializable]
    public class EnemyIdle : BaseEnemyState
    {
        private EnemyStateMachineController _enemyControllerSm;
        private RaycastHit _hit;
        private float _distance;
        private Vector3 _direction;

        public EnemyIdle(EnemyStateMachineController stateMachine) : base("EnemyIdle", stateMachine)
        {
            _enemyControllerSm = (EnemyStateMachineController)stateMachine;
        }

        public override void Enter()
        {
            _enemyControllerSm.animator.SetBool("Walk", false);
            _enemyControllerSm.animator.SetBool("Hit", false);
        }

        public override void UpdateLogic()
        {
            

        }

        public override void UpdatePhysics()
        {
           
        }

        public override void UpdateMovement(Transform target)
        {
          
                _distance = Vector3.Distance(_enemyControllerSm.thisTransform.position,
                    target.position);
                if (_distance <= _enemyControllerSm.loocDistance)
                {
                    _direction = (target.position + Vector3.up) -
                                 (_enemyControllerSm.thisTransform.position + Vector3.up);
                    _enemyControllerSm.thisTransform.forward =  -1*_direction.normalized;
                    if (!Physics.Raycast(_enemyControllerSm.thisTransform.position + Vector3.up, _direction, _distance,
                            _enemyControllerSm.layerWall))
                    {
                        _enemyControllerSm.target = target;
                        _enemyControllerSm.targetHealthController =target.GetComponent<AbstractHealth>();
                        _enemyControllerSm.ChangeState(_enemyControllerSm.enemyAttack);
                        return;
                    }
                }
            }
        }

        
    }
