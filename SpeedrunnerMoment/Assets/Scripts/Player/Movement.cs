using UnityEngine;
using Util;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float maxSpeed;

        [Range(0.0f, 0.99f)]
        public float velocitySmoothness;

        
        private const float MaxVelocitySmoothness = 1.0f;

        private ThirdPersonCamera _camera;
        private Rigidbody _rigidbody;

        private Vector3 _movementDirection;

        private float VelocitySmoothness => MaxVelocitySmoothness - velocitySmoothness;
        
        private Vector3 MovementVelocity => maxSpeed * _movementDirection;

        
        private void Start()
        {
            _camera = FindCamera();
            _rigidbody = GetComponent<Rigidbody>();

            _movementDirection = Vector3.zero;
        }
        
        private void Update()
        {
            _movementDirection = GetMovementDirection();
            SmoothMove();
            SynchronizeRotationWithCamera();
        }

        private Vector3 GetMovementDirection()
        {
            var forward = Input.GetAxis("Vertical")   * _camera.GetForward();
            var right   = Input.GetAxis("Horizontal") * _camera.GetRight();
            
            var movementDirection = forward + right;
            return movementDirection.magnitude > 1 ?
                movementDirection.normalized :
                movementDirection;
        }

        private void SmoothMove()
        {
            var targetVelocity = MovementVelocity;
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, VelocitySmoothness);
        }

        private void SynchronizeRotationWithCamera()
        {
            var cameraForward = _camera.GetForward();
            cameraForward.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraForward);
        }

        
        private static ThirdPersonCamera FindCamera()
        {
            var cameras = FindObjectsOfType<ThirdPersonCamera>();
            if (cameras.Length != 0)
            {
                return cameras[0];
            }
            
            Debug.Log("Couldn't find available cameras");
            return null;
        }
    }
}
