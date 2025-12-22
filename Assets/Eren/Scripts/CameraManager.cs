using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("You have more than one CameraManager.");
            Destroy(gameObject);
        }

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

    }

    public void ChangeCameraDistance(float distance) => transposer.m_CameraDistance = distance;


}
