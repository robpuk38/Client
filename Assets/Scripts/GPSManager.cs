using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSManager : MonoBehaviour
{


    public float latitude;
    public float longitude;
    IEnumerator GpsCords;

    private void Start()
    {
        CheckForNewGPSCords();
    }
    int countGpsTimer = 0;
    private void Update()
    {
        if (countGpsTimer < 300)
        {
            countGpsTimer++;
        }
        if (countGpsTimer > 290)
        {
            CheckForNewGPSCords();
            countGpsTimer = 0;
        }

    }

    private void CheckForNewGPSCords()
    {
        GpsCords = GetGpsCords();
        StartCoroutine(GpsCords);
    }
    bool run_once = false;
    bool isfakegps = false;
    private IEnumerator GetGpsCords()
    {
        if (Input.location.isEnabledByUser)
        {
            isfakegps = false;
        }
        if (isfakegps == true)
        {
            yield break;
        }

        if (!Input.location.isEnabledByUser && isfakegps == false)
        {
            float fake_cordsx = Random.Range(0, 5000);
            float fake_cordsy = Random.Range(0, 5000);
            float fake_cordsz = Random.Range(0, 5000);
            isfakegps = true;
            latitude = fake_cordsx + fake_cordsz - fake_cordsy;
            longitude = -fake_cordsx - fake_cordsz + fake_cordsy;
            SetGpsPos();

            // Debug.Log("GPS WE MADE IN ONCE ");
            yield break;

        }

        if (run_once == false)
        {
            Input.location.Start(10.0f, 10.0f);
            run_once = true;

        }
        if (run_once == true)
        {
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            if (maxWait <= 0)
            {
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                yield break;

            }

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            SetGpsPos();
        }


        yield break;
    }

    public float GetGpsX()
    {
        return latitude;
    }
    public float GetGpsY()
    {
        return latitude - longitude;
    }
    public float GetGpsZ()
    {
        return longitude;
    }

    public void SetGpsPos()
    {
        if (DataManager.Instance != null)
        {
            // Debug.Log("DID WE MAKE INSIDE THE GPS DATAMANAGER SYSTEM");
            DataManager.Instance.SetUserGpsX(GetGpsX().ToString());
            DataManager.Instance.SetUserGpsY(GetGpsY().ToString());
            DataManager.Instance.SetUserGpsZ(GetGpsZ().ToString());
        }
    }
}
