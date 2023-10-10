using _Scripts.Player.BasePlayer.BaseStateMachine;


public class BasePlayerState 
{
    public string name;
    protected BasePlayerStateMachine stateMachine;

    public BasePlayerState(string name, BasePlayerControllerSm stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void UpdateMovement() { }
    public virtual void Exit() {}
}
