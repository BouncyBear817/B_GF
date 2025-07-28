using UnityEngine;

namespace GameMain
{
    public sealed partial class CameraManager
    {
        public interface ICameraController
        {
            void Init();
            
            void SetCamera(Camera camera);

            void Update();

            void LateUpdate();
        }
    }
}