using UnityEngine;
using System;
using System.Threading;

public class AndroidManager : MonoBehaviour {

    private static AndroidManager instance;
    public static AndroidManager Instance { get { return instance; } }

    public GameObject CheckNotifications;
    //float sleepUntil = 0;
    void Awake () {
        instance = this;
        CheckNotifications.SetActive(false);
        //new Timer(AleartTime,null,0,10000);
     

    }
   
    //private void AleartTime(object state)
   // {
     //   Debug.Log("TIMER: UPDATE THREADING");
        //GetNotificationsFromServer("0", "1", "MyTitle", "My Message", 20, 1);
       
        //GC.Collect();
   // }
    private int counttime = 0;
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Debug.Log("WE ARE IN THE CLIENT");
            
        }
        else
        {
            counttime++;
            Debug.Log("WE ARE NOT IN THE CLIENT"+ counttime);
            GetNotificationsFromServer("0", "1", "MyTitle", "My Message",20,1);
        }
    }

    private void OnApplicationPause(bool focus)
    {
        if (focus)
        {
            Debug.Log("WE ARE IN THE CLIENT 2");

        }
        else
        {
            counttime++;
            Debug.Log("WE ARE NOT IN THE CLIENT 2 " + counttime);
            GetNotificationsFromServer("0", "1", "MyTitle", "My Message", 20, 1);
        }
    }



    public void GetNotificationOnAwakeRepeating(string to, string from, string title, string message)
    {
        try
        {
            LocalNotification.SendRepeatingNotification(1, 5, 5, title, message, new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
        }catch(Exception ex)
        {
            Debug.Log("WE CRASHED");
        }
        //sleepUntil = Time.time + 5;
        //if (CheckNotifications != null)
        //{
        //    CheckNotifications.SetActive(true);
        //}
    }
    public void SendNotificationsToServer(string to, string from, string title, string message)
    {


    }

    
    public void GetNotificationsFromServer(string from, string to, string title, string message,int time, int id)
    {
        LocalNotification.SendNotification(id, time, title, message, new Color32(0xff, 0x44, 0x44, 255),true,true,true,"app_icon", executeMode: LocalNotification.NotificationExecuteMode.ExactAndAllowWhileIdle);


        //sleepUntil = Time.time + 10;
        //if (CheckNotifications != null)
       // {
       //     CheckNotifications.SetActive(true);
       // }
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
