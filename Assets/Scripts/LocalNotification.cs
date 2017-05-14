using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class LocalNotification
{
  
    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }


    private static string fullClassName = "com.pctrs.unitynotification.UnityNotificationManager";
    private static string mainActivityClassName = "com.unity3d.player.UnityPlayerNativeActivity";


    public static void SendNotification(int id, TimeSpan delay, string title, string message)
    {
        SendNotification(id, (int)delay.TotalSeconds, title, message, Color.white);
    }
    
    public static void SendNotification(int id, long delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", NotificationExecuteMode executeMode = NotificationExecuteMode.Inexact)
    {

        AndroidJavaClass pluginClass;
        try
        {
            pluginClass = new AndroidJavaClass(fullClassName);
            if (pluginClass != null)
            {
                pluginClass.CallStatic("SetNotification", id, delay * 1000L, title, message, message, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, (int)executeMode, mainActivityClassName);
            }

        }
        catch(Exception ex)
        {
            //Debug.Log("Error "+ ex.Message);
        }
        

    }

    public static void SendRepeatingNotification(int id, long delay, long timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {

        
        AndroidJavaClass pluginClass;
        try
        {
            pluginClass = new AndroidJavaClass(fullClassName);
            if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id, delay * 1000L, title, message, message, timeout * 1000, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b, mainActivityClassName);
        }
    }
        catch(Exception ex)
        {
            //Debug.Log("Error "+ ex.Message);
        }

    }

    public static void CancelNotification(int id)
    {

        AndroidJavaClass pluginClass;
        try
        {
            pluginClass = new AndroidJavaClass(fullClassName);
            if (pluginClass != null)
            {
                pluginClass.CallStatic("CancelNotification", id);
            }
        }
        catch (Exception ex)
        {
            //Debug.Log("Error " + ex.Message);
        }
    }

    
}
