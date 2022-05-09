using UnityEngine;

namespace Util
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public GameObject target;
        public Vector3 targetOffset;
    
        public Vector2 cameraOffset;

        [Range(0.1f, 10.0f)]
        public float mouseSensitivity;

        // For position calculation.
        private AircraftAxisVector _lookAtTarget;

        private bool _canRotate;
    
        
        public Vector3 GetForward()
        {
            var forward = transform.forward;
            return new Vector3(forward.x, 0, forward.z).normalized;
        }

        public Vector3 GetRight()
        {
            var right = transform.right;
            return new Vector3(right.x, 0, right.z).normalized;
        }
    
        
        private void Start()
        {
            _lookAtTarget = new AircraftAxisVector
            {
                pitch = Mathf.Atan2(cameraOffset.y, cameraOffset.x) * Mathf.Rad2Deg,
                yaw = 0
            };
            _canRotate = false;
        }


        private void LateUpdate()
        {
            HandleFire2Button();
            HandleCameraRotation();
            MoveAroundTarget();
            LookAtTargetPoint();
        }

        private void HandleFire2Button()
        {
            if (Input.GetButtonDown("Fire2"))
            {
                EnableRotation();
            }

            if (Input.GetButtonUp("Fire2"))
            {
                DisableRotation();
            }
        }
    
        private void EnableRotation()
        {
            _canRotate = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    
        private void DisableRotation()
        {
            _canRotate = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    
        private void HandleCameraRotation()
        {
            if (!_canRotate)
            {
                return;
            }
            
            var rotationAngle = Input.GetAxis("Mouse X") * mouseSensitivity;
            _lookAtTarget.yaw += rotationAngle;
        }
    
        private void MoveAroundTarget()
        {
            var direction = _lookAtTarget.HorizontalForwardDirection();

            var offset3d = -cameraOffset.x * direction;
            offset3d.y = cameraOffset.y;
            
            transform.position = target.transform.position + offset3d;
        }

        private void LookAtTargetPoint()
        {
            var targetPoint = target.transform.position + targetOffset;
            var direction = targetPoint - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}