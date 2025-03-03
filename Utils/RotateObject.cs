using UnityEngine;

// <summary>
// Rotates a GameObject continuously around a specified axis.
// </summary>
// <remarks>
// This component allows for:
// - Selection of rotation axis (X, Y, or Z)
// - Customizable rotation speed
// - Continuous rotation in Update
// 
// The rotation is applied to the GameObject this component is attached to.
// Positive speed values rotate clockwise, negative values rotate counter-clockwise.
// </remarks>

namespace OghiUnityTools.Utils
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Rotation speed in degrees per second. Use negative values for counter-clockwise rotation.")]
        [Range(-360f, 360f)]
        private float speed = 90f;

        [SerializeField] [Tooltip("The axis around which the object will rotate")]
        private AxisToRotateAround axisToRotateAround = AxisToRotateAround.Y;

        [SerializeField] [Tooltip("Controls whether the rotation is active")]
        private bool isRotating = true;

        private Transform objectToRotate;
        private Vector3 rotationVector;

        /// <summary>
        /// Defines the available axes for rotation.
        /// </summary>
        public enum AxisToRotateAround
        {
            /// <summary>Rotate around the X axis</summary>
            X,

            /// <summary>Rotate around the Y axis</summary>
            Y,

            /// <summary>Rotate around the Z axis</summary>
            Z
        }

        /// <summary>
        /// Gets or sets the rotation speed in degrees per second.
        /// </summary>
        public float Speed
        {
            get => speed;
            set => speed = Mathf.Clamp(value, -360f, 360f);
        }

        /// <summary>
        /// Gets or sets the axis of rotation.
        /// </summary>
        public AxisToRotateAround RotationAxis
        {
            get => axisToRotateAround;
            set
            {
                axisToRotateAround = value;
                UpdateRotationVector();
            }
        }

        /// <summary>
        /// Gets or sets whether the object is currently rotating.
        /// </summary>
        public bool IsRotating
        {
            get => isRotating;
            set => isRotating = value;
        }

        private void Awake()
        {
            objectToRotate = this.transform;
            UpdateRotationVector();
        }

        private void Update()
        {
            if (!isRotating) return;
            objectToRotate.Rotate(rotationVector * (speed * Time.deltaTime));
        }

        private void Reset()
        {
            speed = 90f;
            axisToRotateAround = AxisToRotateAround.Y;
            isRotating = true;
            UpdateRotationVector();
        }

        /// <summary>
        /// Updates the rotation vector based on the selected axis.
        /// </summary>
        private void UpdateRotationVector()
        {
            rotationVector = axisToRotateAround switch
            {
                AxisToRotateAround.X => Vector3.right,
                AxisToRotateAround.Y => Vector3.up,
                AxisToRotateAround.Z => Vector3.forward,
                _ => Vector3.up
            };
        }

        /// <summary>
        /// Pauses the rotation.
        /// </summary>
        public void PauseRotation() => isRotating = false;

        /// <summary>
        /// Resumes the rotation.
        /// </summary>
        public void ResumeRotation() => isRotating = true;

        /// <summary>
        /// Toggles the rotation state between paused and rotating.
        /// </summary>
        public void ToggleRotation() => isRotating = !isRotating;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                UpdateRotationVector();
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Calculate the rotation vector if not in play mode
            Vector3 currentRotationVector = Application.isPlaying
                ? rotationVector
                : axisToRotateAround switch
                {
                    AxisToRotateAround.X => Vector3.right,
                    AxisToRotateAround.Y => Vector3.up,
                    AxisToRotateAround.Z => Vector3.forward,
                    _ => Vector3.up
                };

            // Draw rotation axis indicator
            Gizmos.color = Color.yellow;
            Vector3 start = transform.position;
            Vector3 end = start + currentRotationVector * 2f;
            Gizmos.DrawLine(start, end);

            // Draw arrow head
            Gizmos.DrawWireSphere(end, 0.1f);

            // Draw rotation direction arc
            UnityEditor.Handles.color = new Color(1f, 1f, 0f, 0.2f);
            UnityEditor.Handles.DrawWireArc(
                transform.position,
                currentRotationVector,
                Vector3.Cross(currentRotationVector, Vector3.up),
                speed > 0 ? 90 : -90,
                1f
            );
        }
#endif
    }
}