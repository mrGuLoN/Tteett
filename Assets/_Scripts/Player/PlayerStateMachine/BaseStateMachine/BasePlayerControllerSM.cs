using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Player.BasePlayer.BaseStateMachine
{
    public class BasePlayerControllerSm : BasePlayerStateMachine
    {
        public CharacterController characterController => _characterController;
        public Animator animator => _animator;
        public Transform thisTransform => _thisTransform;
        public float nonGunSpeed => _nonGunSpeed;
        public float gunSpeed => _gunSpeed;
        
        [HideInInspector] public BaseNonGun nonGunState;
        [HideInInspector] public BaseGunState gunState;
        public Joystick movementJoyStick;
        public Joystick fireJoyStick;
        [HideInInspector] public  Camera camera;


        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _thisTransform;
        [SerializeField] private float _nonGunSpeed, _gunSpeed;
        private int _chooseCharacter, _chooseMaterial;


        private void Start()
        {
            nonGunState = new BaseNonGun(this);
            gunState = new BaseGunState(this);
            movementJoyStick = CanvasSingleTone.instance.movementJoyStick;
            fireJoyStick = CanvasSingleTone.instance.fireJoyStick;
            ChangeState(nonGunState);
        }

        
        protected override BasePlayerState GetInitialState()
        {
            return nonGunState;
        }

    }
}
