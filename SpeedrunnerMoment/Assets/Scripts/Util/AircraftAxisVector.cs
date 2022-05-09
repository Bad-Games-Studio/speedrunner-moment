using UnityEngine;

namespace Util
{
    /// <summary>
    /// Used to store camera rotations.
    /// </summary>
    public class AircraftAxisVector
    {
        /// <summary>
        /// Up-down rotation (from `-90` to `90` degrees, look up and down respectively).
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public float pitch;
        
        /// <summary>
        /// Left-right rotation: (from `-180` to `180` degrees).
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public float yaw;

        /// <summary>
        /// Rotation around looking direction (from `0` to `360` degrees).
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public float roll;
        
        
        public AircraftAxisVector() : this(0, 0)
        {}
        
        public AircraftAxisVector(float pitch, float yaw, float roll = 0)
        {
            this.pitch = pitch;
            this.yaw = yaw;
            this.roll = roll;
        }

        public Vector3 HorizontalForwardDirection()
        {
            var angle = yaw * Mathf.Deg2Rad;
            var direction = new Vector3
            {
                x = Mathf.Sin(angle),
                y = 0,
                z = Mathf.Cos(angle)
            };

            return direction.normalized;
        }

        public Quaternion ToQuaternion()
        {
            return Quaternion.Euler(pitch, yaw, roll);
        }
    }
}