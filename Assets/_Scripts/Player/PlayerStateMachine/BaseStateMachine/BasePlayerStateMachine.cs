

using Mirror;
using UnityEngine;

public class BasePlayerStateMachine : NetworkBehaviour
{
    private BasePlayerState _currentState;
    private void Start()
    {
        _currentState = GetInitialState();
        if (_currentState != null) _currentState.Enter();
    }
      
    public void UpdateSM()
    {
        if (_currentState != null)
        {
            _currentState.UpdateLogic();
        }
    } 
        
    public void LateUpdateSM()
    {
        if (_currentState != null)
        {
            _currentState.UpdatePhysics();
        }
    }

    public void FixedUpdateSM()
    {
        if (_currentState != null)
        {
            _currentState.UpdateMovement();
        }
    }

    public void ChangeState(BasePlayerState newState)
    {
        if (_currentState!=null)_currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected virtual BasePlayerState GetInitialState()
    {
        return null;
    }
}
