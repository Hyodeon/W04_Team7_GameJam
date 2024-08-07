using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private float _cameraWeight = 1.0f;

    private int _count = 0;

    private float _defaultCameraDistance;
    private float _defaultShoulderOffset;

    public float CameraWeight { 
        get { return _cameraWeight; } 
        set { _cameraWeight = UpdateCameraWeight(value); }
    }

    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        _defaultCameraDistance = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
        _defaultShoulderOffset = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y;

        CameraWeight = 1.0f;
    }

    private float UpdateCameraWeight(float value)
    {
        _cameraWeight = value;

        _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = _defaultCameraDistance * _cameraWeight;
        _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.y = _defaultShoulderOffset * _cameraWeight;

        return value;
    }
    
    public void AddFollower()
    {
        _count++;
        CameraWeight = 1 + (float)_count / 100f;
    }

    public void DeleteFollower()
    {
        _count--;
        CameraWeight = 1 + (float)_count / 100f;
    }

}
