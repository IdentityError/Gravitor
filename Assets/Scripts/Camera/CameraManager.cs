﻿using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance = null;
    public static CameraManager GetInstance() { return instance; }

    [System.Serializable]
    public struct CameraInfo
    {
        public string id;
        public Camera camera;
        public float initFOV;
    }

    [SerializeField] private bool randomizeSkyboxRotation = false;
    public CameraInfo[] cameras;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        int length = cameras.Length;
        for (int i = 0; i < length; i++)
        {
            cameras[i].camera.fieldOfView = cameras[i].initFOV;
        }
    }

    private void Start()
    {
        if (randomizeSkyboxRotation)
        {
            RenderSettings.skybox.SetFloat("_Rotation", UnityEngine.Random.Range(0f, 360f));
        }
    }
    //
    public void SmoothInAndOutFOV(string cameraID, float targetFOV, float transitionDuration, float duration)
    {
        if (cameraID == null)
        {
            int length = cameras.Length;
            for (int i = 0; i < length; i++)
            {
                StartCoroutine(SmoothInAndOutFOVCoroutine(targetFOV, transitionDuration, duration, cameras[i]));
            }
        }
        else
        {
            CameraInfo cameraInfo = Array.Find(cameras, cam => cam.id == cameraID);
            StartCoroutine(SmoothInAndOutFOVCoroutine(targetFOV, transitionDuration, duration, cameraInfo));
        }
    }
    private IEnumerator SmoothInAndOutFOVCoroutine(float targetFOV, float transitionDuration, float duration, CameraInfo cameraInfo)
    {
        float initFOV;
        float currentFOV = initFOV = cameraInfo.camera.fieldOfView;

        float stride = Time.fixedDeltaTime / transitionDuration;
        stride *= Mathf.Abs(currentFOV - targetFOV);
        if (targetFOV > currentFOV)
        {
            while (currentFOV + stride <= targetFOV)
            {

                currentFOV += stride;
                cameraInfo.camera.fieldOfView = currentFOV;
                yield return new WaitForFixedUpdate();
            }
            cameraInfo.camera.fieldOfView = currentFOV = targetFOV;

            yield return new WaitForSeconds(duration);

            while (currentFOV - stride >= initFOV)
            {
                currentFOV -= stride;
                cameraInfo.camera.fieldOfView = currentFOV;
                yield return new WaitForFixedUpdate();
            }
            cameraInfo.camera.fieldOfView = initFOV;
        }

        else if (targetFOV < currentFOV)
        {
            while (currentFOV - stride >= targetFOV)
            {
                currentFOV -= stride;
                cameraInfo.camera.fieldOfView = currentFOV;
                yield return new WaitForFixedUpdate();
            }
            cameraInfo.camera.fieldOfView = targetFOV;

            yield return new WaitForSeconds(duration);

            while (currentFOV + stride >= initFOV)
            {
                currentFOV += stride;
                cameraInfo.camera.fieldOfView = currentFOV;
                yield return new WaitForFixedUpdate();
            }
            cameraInfo.camera.fieldOfView = initFOV;
        }
    }

    public void SmoothFOV(string cameraID, float targetFOV, float transitionDuration, float delay)
    {
        if (cameraID == null)
        {
            int length = cameras.Length;
            for (int i = 0; i < length; i++)
            {
                StartCoroutine(SmoothFOVCoroutine(targetFOV, transitionDuration, delay, cameras[i]));
            }
        }
        else
        {
            CameraInfo cameraInfo = Array.Find(cameras, cam => cam.id == cameraID);
            StartCoroutine(SmoothFOVCoroutine(targetFOV, transitionDuration, delay, cameraInfo));
        }
    }
    private IEnumerator SmoothFOVCoroutine(float targetFOV, float transitionDuration, float delay, CameraInfo cameraInfo)
    {
        float currentFOV = cameraInfo.camera.fieldOfView;
        float stride = Time.fixedDeltaTime / Mathf.Abs(currentFOV - targetFOV);

        yield return new WaitForSeconds(delay);
        if (targetFOV > currentFOV)
        {
            while (currentFOV + stride <= targetFOV)
            {
                currentFOV += stride;
                yield return new WaitForFixedUpdate();
                cameraInfo.camera.fieldOfView = currentFOV;
            }
            cameraInfo.camera.fieldOfView = targetFOV;
        }
        else if (targetFOV < currentFOV)
        {
            while (currentFOV + stride >= targetFOV)
            {
                currentFOV -= stride;
                yield return new WaitForFixedUpdate();
                cameraInfo.camera.fieldOfView = currentFOV;
            }
            cameraInfo.camera.fieldOfView = targetFOV;
        }
    }

    public void ResetFOV(string cameraID)
    {
        if (cameraID == null)
        {
            int length = cameras.Length;
            for (int i = 0; i < length; i++)
            {
                cameras[i].camera.fieldOfView = cameras[i].initFOV;
            }
        }
        else
        {
            CameraInfo cameraInfo = Array.Find(cameras, cam => cam.id == cameraID);
            cameraInfo.camera.fieldOfView = cameraInfo.initFOV;
        }
    }
}
