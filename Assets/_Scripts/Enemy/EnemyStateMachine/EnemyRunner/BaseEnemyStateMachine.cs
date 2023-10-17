using Enemy.StateMachine.States;
using UnityEngine;

public class BaseEnemyStateMachine : MonoBehaviour
{
    protected BaseEnemyState _currentState;
    private void Start()
    {
        _currentState = GetInitialState();
        if (_currentState != null) _currentState.Enter();
    }
      
    public void EveryFrameUpdate()
    {
        if (_currentState != null)
        {
            _currentState.UpdateLogic();
        }
    } 
        
    public void EndEveryFrameUpdateLateUpdate()
    {
        if (_currentState != null)
        {
            _currentState.UpdatePhysics();
        }
    }

    public void Update60FixedUpdate(Transform target)
    {
        if (_currentState != null)
        {
            _currentState.UpdateMovement(target);
        }
    }

    public void ChangeState(BaseEnemyState newState)
    {
        if (_currentState!=null)_currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected virtual BaseEnemyState GetInitialState()
    {
        return null;
    }
}
