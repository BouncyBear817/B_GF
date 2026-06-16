using System;
using GameMain;
using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    public Transform Target;
    public Transform Camera;
    
    private CameraManager mCameraManager;
    
    private void Awake()
    {
        mCameraManager = new CameraManager();
        mCameraManager.Init(Camera, Target);
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