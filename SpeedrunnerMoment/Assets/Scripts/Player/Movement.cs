using System;
using UnityEngine;
using Util;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [Range(0.0f, 10.0f)]
        public float maxSpeed;

        [Range(0.0f, 1.0f)]
        public float strafeSpeedFraction;
        
        [Range(0.0f, 1.0f)]
        public float backwardSpeedFraction;
        
        [Range(0.0f, 3.0f)]
        public float secondsToFullSpeed;


        public delegate void GetAxisInput(Vector3 input);
        public event GetAxisInput OnAxisInputHandled;

        
        private const float MaxAxisInputValue = 1.0f;
        
        private Vector3 _currentAxisInput;
        private float ForwardAxisInput => 
            _currentAxisInput.z > 0.0f
            ? _currentAxisInput.z
            : _currentAxisInput.z * backwardSpeedFraction;
        private float RightAxisInput => _currentAxisInput.x * strafeSpeedFraction;


        private ThirdPersonCamera _camera;
        private Rigidbody _rigidbody;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentAxisInput = Vector3.zero;
        }

        private void Start()
        {
            _camera = FindCamera();
        }

        private void Update()
        {
            UpdateVelocity();
            SynchronizeRotationWithCamera();
        }


        private void UpdateAxisInput()
        {
            var forward  = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            var backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            var left     = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            var right    = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

            var vertical = Convert.ToInt32(forward) - Convert.ToInt32(backward);
            var horizontal = Convert.ToInt32(right) - Convert.ToInt32(left);

            _currentAxisInput.z = ModifyAxisInputValue(_currentAxisInput.z, vertical);
            _currentAxisInput.x = ModifyAxisInputValue(_currentAxisInput.x, horizontal);
            
            var alteredInput = new Vector3(RightAxisInput, 0, ForwardAxisInput);
            
            OnAxisInputHandled?.Invoke(
                alteredInput.magnitude > 1.0f ? alteredInput.normalized : alteredInput);
        }

        private float ModifyAxisInputValue(float currentValue, float axisInput)
        {
            var fps = 1.0f / Time.deltaTime;
            var sensitivity = MaxAxisInputValue / (secondsToFullSpeed * fps);
            
            var newValue = currentValue;
            
            if (axisInput > 0.0f)
            {
                newValue += 2 * sensitivity;
                return newValue > MaxAxisInputValue ? MaxAxisInputValue : newValue;
            }

            if (axisInput < 0.0f)
            {
                newValue -= 2 * sensitivity;
                return newValue < -MaxAxisInputValue ? -MaxAxisInputValue : newValue;
            }

            if (currentValue > 0.0f)
            {
                newValue -= sensitivity;
                return newValue < 0.0f ? 0.0f : newValue;
            }

            if (currentValue < 0.0f)
            {
                newValue += sensitivity;
                return newValue > 0.0f ? 0.0f : newValue;
            }

            return 0.0f;
        }
        
        
        private Vector3 GetMovementDirection()
        {
            UpdateAxisInput();

            var forward = ForwardAxisInput * _camera.GetForward();
            var right = RightAxisInput * _camera.GetRight();
            
            var newMovementDirection = forward + right;
            
            return newMovementDirection.magnitude > 1 ?
                newMovementDirection.normalized :
                newMovementDirection;
        }

        private void UpdateVelocity()
        {
            _rigidbody.velocity = maxSpeed * GetMovementDirection();
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
