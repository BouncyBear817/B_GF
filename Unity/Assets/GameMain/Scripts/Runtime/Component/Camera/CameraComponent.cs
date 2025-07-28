using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CameraComponent : GameFrameworkComponent
    {
        private ICameraManager mCameraManager;
        
        protected override void Awake()
        {
            base.Awake();

            mCameraManager = new CameraManager();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(Transform camera, Transform target)
        {
            mCameraManager.Init(camera, target);
        }

        /// <summary>
        /// 设置摄像机控制器
        /// </summary>
        /// <param name="cameraControllerType"></param>
        public void SetCameraController(CameraControllerType cameraControllerType)
        {
            mCameraManager.SetCameraController(cameraControllerType);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            mCameraManager.Dispose();
        }
    }
}