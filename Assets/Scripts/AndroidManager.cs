using UnityEngine;
using System;
using System.Threading;

public class AndroidManager : MonoBehaviour {

    private static AndroidManager instance;
    public static AndroidManager Instance { get { return instance; } }

    private static string AndroidClassPackage = "com.pctrs.unitynotification.UnityNotifincationsActivity";
    private static string MainUnityNativeActivity = "com.unity3d.player.UnityPlayerNativeActivity";

    public GameObject CheckNotifications;
    
    void Awake () {
        instance = this;
        CheckNotifications.SetActive(true);

        SendNotification(200);

    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
           // Debug.Log("MYAPPTEST" + " WE ARE PAUSED AS TRUE");
        }
        else
        {
            //Debug.Log("MYAPPTEST" + " WE ARE NOT PAUSED SO ITS  FALSE");
        }

    }

    private void OnApplicationQuit()
    {
       // Debug.Log("MYAPPTEST" + " WE HAVE QUIT THE APP");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            // this is true
            
           // Debug.Log("MYAPPTEST" + " WE ARE FOUCSED THIS MEANS WE ARE IN THE APP");
        }
        else
        {
            // this is false
            SendNotification(300);
           
           // Debug.Log("MYAPPTEST" + " WE ARE NOT FOUCUSED MEANS WE ARE IN THE BACKGROUND BUT STILL RUNNING");
        }
    }


    public static void SendNotification(int id)
    {
        //Debug.Log("MYAPPTEST" + " WE ARE SENDING A NOTIFICATION THOUGH ");

        AndroidJavaClass OurNotificationsPlugin;

        try
        {
            OurNotificationsPlugin = new AndroidJavaClass(AndroidClassPackage);

            if (OurNotificationsPlugin != null)
            {
                // we are even better to go now becaue it has access

                String SIconImage = "notify_icon_big";
                String LIconImage = "notify_icon_big";
                Color32 bgColor = new Color32(0xff, 0x44, 0x44, 255);
                int executeMode = 2;
                bool sound = true;
                bool lights = true;
                bool vibration = true;
                String title = "MY TITLE 3 HOUR REMINDER";
                String message = "My Messsage Boo";
                String ticker = message;
                long delaytime = 10800; // 3 hours 
               // Debug.Log("MYAPPTEST" + "  we are even better to go now becaue it has access ");
                OurNotificationsPlugin.CallStatic("GetNotification", id, delaytime * 1000L, title, message, ticker, sound ? 1 : 0, vibration ? 1 : 0, lights ? 0 : 1, SIconImage, LIconImage, bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, executeMode, MainUnityNativeActivity);
            }
            else
            {
                //damit we have a error again but its not from the android java calss its we have a null
               // Debug.Log("MYAPPTEST" + "  damit we have a error again but its not from the android java calss its we have a null ");
            }

            // hey we are good to go lets do it
        }
        catch (AndroidJavaException ex)
        {
            // fuck we crashed
            //Debug.Log("MYAPPTEST" + "   fuck we crashed " + ex.Message);
        }
        catch (Exception ex)
        {
            // dam we crashed from some exeption 
           // Debug.Log("MYAPPTEST" + "   dam we crashed from some exeption  " + ex.Message);
        }

    }


    public void ClientCheckNotificationsBtn()
    {
        if (CheckNotifications.activeSelf == true)
        {
            CheckNotifications.SetActive(false);
        }

       
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
                                //Debug.Log("ANDROID ERROR 3: " + ex.Message);
                                ServerStatusManager.Instance.Message.text = ex.Message;
                            }
                        }
                        catch (Exception ex)
                        {
                            //Debug.Log("ANDROID ERROR 4: " + ex.Message);
                            ServerStatusManager.Instance.Message.text = ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Debug.Log("ANDROID ERROR 5: " + ex.Message);
                        ServerStatusManager.Instance.Message.text = ex.Message;
                    }
                }
                catch (Exception ex)
                {
                   // Debug.Log("ANDROID ERROR 6: " + ex.Message);
                    ServerStatusManager.Instance.Message.text = ex.Message;
                }

            }
            catch (Exception ex)
            {
                //Debug.Log("ANDROID ERROR 7: " + ex.Message);
                ServerStatusManager.Instance.Message.text = ex.Message;
            }
        }
        catch (Exception ex)
        {
            //Debug.Log("ANDROID ERROR 8: " + ex.Message);
            ServerStatusManager.Instance.Message.text = ex.Message;
        }
        DataManager.Instance.SetUserDeviceId(android_id);
        return android_id;


    }
}
