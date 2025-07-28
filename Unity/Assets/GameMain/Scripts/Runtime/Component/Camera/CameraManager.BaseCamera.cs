using GameFramework;
using UnityEngine;

namespace GameMain
{
    public sealed partial class CameraManager
    {
        public abstract class BaseCameraController : MonoBehaviour, ICameraController
        {
            protected Camera mCamera;

            public virtual void Init()
            {
                if (mCamera == null)
                {
                    throw new GameFrameworkException("Camera not initialized, Please use 'SetCamera' first.");
                }
            }

            public void SetCamera(Camera camera)
            {
                mCamera = camera;
            }

            public virtual void Update()
            {
                if (mCamera == null)
                {
                    return;
                }
                
                if (Input.GetMouseButtonDown(1))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }

                if (Input.GetMouseButtonUp(1))
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }

            public virtual void LateUpdate()
            {
                if (mCamera == null)
                {
                    return;
                }
            }
        }
    }
}