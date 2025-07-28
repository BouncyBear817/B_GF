using UnityEngine;

namespace GameMain
{
    public sealed partial class CameraManager
    {
        public class ObjectCameraController : BaseCameraController
        {
            private Transform mTarget;
            public int MouseWheelSensitivity = 1;
            public int MouseZoomMin = 1;
            public int MouseZoomMax = 5;
            public float normalDistance = 3;
            
            [Header("Movement Settings")] [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
            public float boost = 3.5f;
            
            [Header("Rotation Settings")] [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
            public AnimationCurve mouseSensitivityCurve =
                new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

            private Vector3 mScreenPoint;
            private Vector3 offset;

            public void SetTarget(Transform target)
            {
                mTarget = target;
            }

            public override void Init()
            {
                base.Init();

                var z = mTarget.transform.position.z - normalDistance;
                mCamera.transform.position = Quaternion.Euler(Vector3.zero) * new Vector3(mCamera.transform.position.x, mCamera.transform.position.y, z);

                mCamera.transform.LookAt(mTarget);
            }

            public override void LateUpdate()
            {
                base.LateUpdate();

                if (Input.GetMouseButton(1))
                {
                    var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                    transform.RotateAround(mTarget.transform.position, Vector3.up, mouseMovement.x * mouseSensitivityFactor);
                    transform.RotateAround(mTarget.transform.position, transform.right, -mouseMovement.y * mouseSensitivityFactor);
                }

                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    var normalized = (mCamera.transform.position - mTarget.position).normalized;

                    if (normalDistance >= MouseZoomMin && normalDistance <= MouseZoomMax)
                    {
                        normalDistance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
                    }

                    normalDistance = Mathf.Clamp(normalDistance, MouseZoomMin, MouseZoomMax);

                    mCamera.transform.position = normalized * normalDistance + mTarget.position;
                }

                if (Input.GetMouseButtonDown(2))
                {
                    mScreenPoint = mCamera.WorldToScreenPoint(mTarget.transform.position);
                    offset = mTarget.transform.position - mCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mScreenPoint.z));
                }

                if (Input.GetMouseButton(2))
                {
                    var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mScreenPoint.z);

                    var curPosition = mCamera.ScreenToWorldPoint(curScreenPoint) + offset;
                    mTarget.transform.position = curPosition;
                }
            }
        }
    }
}