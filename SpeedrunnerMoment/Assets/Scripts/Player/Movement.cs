using System;
using UnityEngine;
using Util;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float maxSpeed;
        
        [Range(1.0f, 10.0f)]
        public float velocitySmoothness;
        private float _axisInputSensitivity;


        public delegate void AxisInput(Vector3 input);
        public event AxisInput OnAxisInputObtained;

        
        private Vector3 _currentAxisInput;
        private const float MaxAxisInputValue = 1.0f;


        private ThirdPersonCamera _camera;
        private Rigidbody _rigidbody;

        private Vector3 _movementDirection;

        
        private Vector3 MovementVelocity => maxSpeed * _movementDirection;

        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentAxisInput = Vector3.zero;

            var a = GetComponentsInChildren<Collider>();
            Debug.Log(a.Length);

            _movementDirection = Vector3.zero;
        }

        private void Start()
        {
            _camera = FindCamera();
        }

        private void Update()
        {
            _axisInputSensitivity = MaxAxisInputValue / (velocitySmoothness * 60.0f);
            _movementDirection = GetMovementDirection();
            SmoothMove();
            SynchronizeRotationWithCamera();
        }

        private Vector3 ExtractAxisInput()
        {
            var forward  = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            var backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            var left     = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            var right    = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

            var vertical = Convert.ToInt32(forward) - Convert.ToInt32(backward);
            var horizontal = Convert.ToInt32(right) - Convert.ToInt32(left);

            _currentAxisInput.z = ModifyAxisInput(_currentAxisInput.z, vertical);
            _currentAxisInput.x = ModifyAxisInput(_currentAxisInput.x, horizontal);

            OnAxisInputObtained?.Invoke(
                _currentAxisInput.magnitude > 1.0f ? _currentAxisInput.normalized : _currentAxisInput);

            return _currentAxisInput;
        }

        private float ModifyAxisInput(float currentValue, float axisInput)
        {
            var newValue = currentValue;
            
            if (axisInput > 0.0f)
            {
                newValue += _axisInputSensitivity;
                return newValue > MaxAxisInputValue ? MaxAxisInputValue : newValue;
            }

            if (axisInput < 0.0f)
            {
                newValue -= _axisInputSensitivity;
                return newValue < -MaxAxisInputValue ? -MaxAxisInputValue : newValue;
            }

            if (currentValue > 0.0f)
            {
                newValue -= _axisInputSensitivity;
                return newValue < 0.0f ? 0.0f : newValue;
            }

            if (currentValue < 0.0f)
            {
                newValue += _axisInputSensitivity;
                return newValue > 0.0f ? 0.0f : newValue;
            }

            return 0.0f;
        }
        
        private Vector3 GetMovementDirection()
        {
            var axisInput = ExtractAxisInput();
            var forward = axisInput.z * _camera.GetForward();
            var right   = axisInput.x * _camera.GetRight();
            
            var movementDirection = forward + right;
            return movementDirection.magnitude > 1 ?
                movementDirection.normalized :
                movementDirection;
        }

        private void SmoothMove()
        {
            var targetVelocity = MovementVelocity;
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, 0.75f);
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
