using _Scripts.Player.BasePlayer.BaseStateMachine;
using UnityEngine;

public class BaseNonGun : BasePlayerState
{
    private BasePlayerControllerSm _pSm;
    private Vector3 _charMovement;
    public BaseNonGun(BasePlayerControllerSm stateMachine) : base("BaseNonGun", stateMachine)
    {
        _pSm = (BasePlayerControllerSm)stateMachine;
    }

   
    public override void Enter()
    {
       _pSm.animator.SetBool("Walk", false);
    }

    public override void UpdateLogic()
    {
        if (_pSm.fireJoyStick.Direction != Vector2.zero)
        {
            _pSm.ChangeState(_pSm.gunState);
        }
    }

    public override void UpdateMovement()
    {
        if (_pSm.movementJoyStick.Direction != Vector2.zero)
        {
            _pSm.animator.SetFloat("InputY", 1);
            _charMovement = new Vector3(_pSm.movementJoyStick.Direction.x, 0, _pSm.movementJoyStick.Direction.y);
            _pSm.thisTransform.forward = _charMovement.normalized;
            _pSm.characterController.Move(_charMovement.normalized * (Time.deltaTime * _pSm.nonGunSpeed)+Vector3.down);
        }
        else
        {
            _pSm.animator.SetFloat("InputY", 0);
        }
    }
}
