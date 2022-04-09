using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Movement))]
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Movement _movement;

        private int ForwardVelocityParameter { get; } = Animator.StringToHash("ForwardVelocity");
        private int RightVelocityParameter { get; }   = Animator.StringToHash("RightVelocity");
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            _movement.OnAxisInputHandled += UpdateAnimation;
        }

        private void OnDisable()
        {
            _movement.OnAxisInputHandled -= UpdateAnimation;
        }

        private void UpdateAnimation(Vector3 axisInputs)
        {
            var forward = axisInputs.z;
            var right   = axisInputs.x;
            
            _animator.SetFloat(ForwardVelocityParameter, forward);
            _animator.SetFloat(RightVelocityParameter,   right);
        }
    }
}
