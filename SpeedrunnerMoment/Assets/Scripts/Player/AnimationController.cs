using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Movement))]
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Movement _movement;

        private int _forwardVelocityParameter;
        private int _rightVelocityParameter;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<Movement>();

            _forwardVelocityParameter = Animator.StringToHash("ForwardVelocity");
            _rightVelocityParameter   = Animator.StringToHash("RightVelocity");
        }

        private void OnEnable()
        {
            _movement.OnAxisInputObtained += UpdateAnimation;
        }

        private void OnDisable()
        {
            _movement.OnAxisInputObtained -= UpdateAnimation;
        }

        private void UpdateAnimation(Vector3 axisInputs)
        {
            var forward = axisInputs.z;
            var right   = axisInputs.x;
            
            _animator.SetFloat(_forwardVelocityParameter, forward);
            _animator.SetFloat(_rightVelocityParameter,   right);
        }
    }
}
