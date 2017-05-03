using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour {

    private Gyroscope Gyro;
    private GameObject CamConatiner;
    private Quaternion Roatation;
    private bool isGyroReady = false;
    
    private new WebCamTexture camera;
    public RawImage CameraBackGround;
    public AspectRatioFitter fit;

    private void Start()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            return;
        }


        for(int i = 0; i < WebCamTexture.devices.Length; i++)
        {

            if(!WebCamTexture.devices[i].isFrontFacing)
            {
                camera = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
                break;
            }

        }

        if(camera == null)
        {
            return;
        }


        CamConatiner = new GameObject("Camera Container");
        CamConatiner.transform.position = this.transform.position;
        this.transform.SetParent(CamConatiner.transform);

        Gyro = Input.gyro;
        Gyro.enabled = true;

        CamConatiner.transform.rotation = Quaternion.Euler(90f, 0, 0);
        Roatation = new Quaternion(0, 0, 1, 0);
        camera.Play();
        CameraBackGround.texture = camera;
        isGyroReady = true;
        

    }

    private void FixedUpdate()
    {
        if (isGyroReady == true && SystemInfo.supportsGyroscope)
        {
            float Ratio = (float)camera.width / (float)camera.height;
            fit.aspectRatio = Ratio;
            float ScaleY = camera.videoVerticallyMirrored ? -1.0f : 1.0f;
            CameraBackGround.rectTransform.localScale = new Vector3(1f,ScaleY,1f);
            int Orientaion = -camera.videoRotationAngle;
            CameraBackGround.rectTransform.localEulerAngles = new Vector3(0,0, Orientaion);
            this.transform.localRotation = Gyro.attitude * Roatation;
        }
    }

   
}
