using UnityEngine;

namespace GameMain
{
    public sealed partial class CameraManager
    {
        public partial class FirstCameraController : BaseCameraController
        {
            private CameraState m_TargetCameraState = new CameraState();
            private CameraState m_InterpolatingCameraState = new CameraState();

            [Header("Movement Settings")] [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
            public float boost = 3.5f;

            [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
            public float positionLerpTime = 0.2f;

            [Header("Rotation Settings")] [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
            public AnimationCurve mouseSensitivityCurve =
                new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

            [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
            public float rotationLerpTime = 0.01f;

            [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
            public bool invertY = false;

            public override void Init()
            {
                base.Init();

                m_TargetCameraState.SetFromTransform(mCamera.transform);
                m_InterpolatingCameraState.SetFromTransform(mCamera.transform);
            }

            public override void Update()
            {
                base.Update();
                
                // Rotation
                if (Input.GetMouseButton(1))
                {
                    var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

                    var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                    m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
                    m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
                }

                // Translation
                var translation = GetInputTranslationDirection() * Time.deltaTime;

                // Speed up movement when shift key held
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    translation *= 10.0f;
                }

                // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel)
                boost += Input.mouseScrollDelta.y * 0.2f;
                translation *= Mathf.Pow(2.0f, boost);

                m_TargetCameraState.Translate(translation);

                // Framerate-independent interpolation
                // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
                var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
                var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
                m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

                m_InterpolatingCameraState.UpdateTransform(mCamera.transform);
            }

            private Vector3 GetInputTranslationDirection()
            {
                Vector3 direction = new Vector3();
                if (Input.GetKey(KeyCode.W))
                {
                    direction += Vector3.forward;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    direction += Vector3.back;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    direction += Vector3.left;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    direction += Vector3.right;
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    direction += Vector3.down;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    direction += Vector3.up;
                }

                return direction;
            }
        }
    }
}