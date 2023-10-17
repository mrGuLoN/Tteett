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
         _pSm.thisTransform.up = _pSm.fireJoyStick.Direction.normalized;
      }
      else
      {
         _pSm.animator.SetBool("Fire", false);
         _pSm.thisTransform.up = _pSm.fireJoyStick.Direction.normalized;
      }
      
   }

   public override void UpdatePhysics()
   {
     
   }

   public override void UpdateMovement()
   {
      if (_pSm.movementJoyStick.Direction != Vector2.zero)
      {
         _pSm.characterController.Move(_pSm.movementJoyStick.Direction.normalized * (Time.deltaTime * _pSm.gunSpeed));
         _charMovement = _pSm.thisTransform.InverseTransformDirection(_charMovement);
         _pSm.animator.SetFloat("InputX", _charMovement.x);
         _pSm.animator.SetFloat("InputY", _charMovement.z);
      }
      else
      {
         _pSm.animator.SetFloat("InputY", 0);
         _pSm.animator.SetFloat("InputX", 0);
      }
   }
}
