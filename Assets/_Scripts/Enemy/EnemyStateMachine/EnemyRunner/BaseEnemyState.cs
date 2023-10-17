using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyState 
{
    public string name;
    protected EnemyStateMachineController stateMachineController;

    public BaseEnemyState(string name, EnemyStateMachineController stateMachineController)
    {
        this.name = name;
        this.stateMachineController = stateMachineController;
    }
    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void UpdateMovement(Transform target) { }
    public virtual void Exit() {}
}
