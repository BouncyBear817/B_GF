using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 摄像机管理器接口
    /// </summary>
    public interface ICameraManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init(Transform camera, Transform target);
        
        /// <summary>
        /// 设置摄像机控制器
        /// </summary>
        /// <param name="cameraControllerType"></param>
        void SetCameraController(CameraControllerType cameraControllerType);

        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();
    }
}