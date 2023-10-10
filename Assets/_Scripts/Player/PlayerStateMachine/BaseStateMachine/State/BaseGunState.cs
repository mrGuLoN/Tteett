using _Scripts.Player.BasePlayer.BaseStateMachine;
using UnityEngine;

public class BaseGunState : BasePlayerState
{
   private Vector3 _charMovement;
   private BasePlayerControllerSm _pSm;
   public BaseGunState(BasePlayerControllerSm stateMachine) : base("BaseGunState", stateMachine)
   {
      _pSm = (BasePlayerControllerSm)stateMachine;
   }
   public override void Enter()
   {
      _pSm.animator.SetBool("Walk", true);
   }

   public override void UpdateLogic()
   {
      if (_pSm.fireJoyStick.Direction == Vector2.zero)
      {
         _pSm.animator.SetBool("Fire", false);
         _pSm.ChangeState(_pSm.nonGunState);
      }
      else if (Vector2.SqrMagnitude(_pSm.fireJoyStick.Direction) >= 0.25f)
      {
         _pSm.animator.SetBool("Fire", true);
      }
      else
      {
         _pSm.animator.SetBool("Fire", false);
      }
   }

   public override void UpdatePhysics()
   {
     
   }

   public override void UpdateMovement()
   {
      _pSm.thisTransform.forward = new Vector3(_pSm.fireJoyStick.Direction.x, 0, _pSm.fireJoyStick.Direction.y);
     
      if (_pSm.movementJoyStick.Direction != Vector2.zero)
      {
         _charMovement = new Vector3(_pSm.movementJoyStick.Direction.x, 0, _pSm.movementJoyStick.Direction.y);
         _pSm.thisTransform.forward = _charMovement.normalized;
         _pSm.characterController.Move(_charMovement.normalized * (Time.deltaTime * _pSm.gunSpeed));
      }
      else
      {
         _pSm.animator.SetFloat("InputY", 0);
      }
   }
}
