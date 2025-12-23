using System;
using GameMain;
using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    public Transform mTarget;
    public Transform mCamera;
    
    private CameraManager mCameraManager;
    
    private void Awake()
    {
        mCameraManager = new CameraManager();
        mCameraManager.Init(mCamera, mTarget);
        mCameraManager.SetCameraController(CameraControllerType.FirstPerson);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Reset"))
        {
            mCameraManager.Dispose();
        }
    }
}