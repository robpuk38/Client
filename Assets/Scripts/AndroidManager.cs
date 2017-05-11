using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AndroidManager : MonoBehaviour {

    private static AndroidManager instance;
    public static AndroidManager Instance { get { return instance; } }

    void Awake () {
        instance = this;

    }

    public void sendNotifications()
    {
       /* AndroidJavaObject AlearmReciver;
        try
        {
            AlearmReciver = new AndroidJavaObject("com.macaronics.notification.AlarmReceiver");
            try
            {
                if (AlearmReciver != null)
                {
                    AlearmReciver.CallStatic("startAlarm", new object[4] { "WTF", "HELLO", "WORK", 1 });
                }
                else
                {
                    ServerStatusManager.Instance.Message.text = "ITS NULL";
                }
            }
            catch (Exception ex)
            {
                Debug.Log("ANDROID ERROR 1: " + ex.Message);
                ServerStatusManager.Instance.Message.text = ex.Message;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("ANDROID ERROR 2: " + ex.Message);
            ServerStatusManager.Instance.Message.text = ex.Message;
        }*/

    }
    private void Update()
    {
        
    }

    public string GetAndroidDeviceID()
    {

        
       AndroidJavaClass up;
        AndroidJavaObject currentActivity;
        AndroidJavaObject contentResolver;
        AndroidJavaClass secure;
        string android_id = "No Android";
        try
        {
            up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            try
            {
                currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                try
                {
                    contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
                    try
                    {
                        secure = new AndroidJavaClass("android.provider.Settings$Secure");
                        try
                        {
                            android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");
                            try
                            {
                                DataManager.Instance.SetUserDeviceId(android_id);
                                return android_id;
                            }
                            catch (Exception ex)
                            {
                                Debug.Log("ANDROID ERROR 3: " + ex.Message);
                                ServerStatusManager.Instance.Message.text = ex.Message;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("ANDROID ERROR 4: " + ex.Message);
                            ServerStatusManager.Instance.Message.text = ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("ANDROID ERROR 5: " + ex.Message);
                        ServerStatusManager.Instance.Message.text = ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("ANDROID ERROR 6: " + ex.Message);
                    ServerStatusManager.Instance.Message.text = ex.Message;
                }

            }
            catch (Exception ex)
            {
                Debug.Log("ANDROID ERROR 7: " + ex.Message);
                ServerStatusManager.Instance.Message.text = ex.Message;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("ANDROID ERROR 8: " + ex.Message);
            ServerStatusManager.Instance.Message.text = ex.Message;
        }
        DataManager.Instance.SetUserDeviceId(android_id);
        return android_id;


    }
}
