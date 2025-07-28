using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameMain
{
    public sealed partial class CameraManager : ICameraManager
    {
        private Transform mTarget;
        private Camera mCamera;

        public void Init(Transform camera, Transform target)
        {
            mCamera = camera.GetComponent<Camera>();
            mTarget = target;
        }

        public void SetCameraController(CameraControllerType cameraControllerType)
        {
            switch (cameraControllerType)
            {
                case CameraControllerType.ThirdPerson:
                {
                    var thirdCameraController = mCamera.AddComponent<ThirdCameraController>();
                    thirdCameraController.Init();
                    break;
                }
                case CameraControllerType.FirstPerson:
                {
                    var firstCameraController = mCamera.AddComponent<FirstCameraController>();
                    firstCameraController.SetCamera(mCamera);
                    firstCameraController.Init();
                    break;
                }
                case CameraControllerType.Object:
                {
                    var ObjectCameraController = mCamera.AddComponent<ObjectCameraController>();
                    ObjectCameraController.SetTarget(mTarget);
                    ObjectCameraController.SetCamera(mCamera);
                    ObjectCameraController.Init();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(cameraControllerType), cameraControllerType, null);
            }
        }

        public void Dispose()
        {
            if (mCamera != null)
            {
                var baseCamera = mCamera.GetComponent<BaseCameraController>();
                if (baseCamera != null)
                {
                    UnityEngine.Object.Destroy(baseCamera);
                }
            }

            mCamera = null;
            mTarget = null;
        }
    }
}