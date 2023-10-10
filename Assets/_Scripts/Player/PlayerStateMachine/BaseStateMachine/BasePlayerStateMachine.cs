

using UnityEngine;

public class BasePlayerStateMachine : MonoBehaviour
{
    private BasePlayerState _currentState;
    private void Start()
    {
        _currentState = GetInitialState();
        if (_currentState != null) _currentState.Enter();
    }
      
    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateLogic();
        }
    } 
        
    private void LateUpdate()
    {
        if (_currentState != null)
        {
            _currentState.UpdatePhysics();
        }
    }

    private void FixedUpdate()
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
