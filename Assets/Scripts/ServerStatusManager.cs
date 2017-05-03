using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
//using System.Threading;

public class ServerStatusManager : MonoBehaviour
{

    public GameObject LoadedSystem;
    public Text Message;
    public GameObject BG;
    public string IpAddress = "127.0.0.1";
    public int port = 3000;
    private string blowfishkey = "c2VydmVyIGNvbm5lY3Rpb24gdHlwZQ==";




    private bool ServerStatus = false;

    private void ServerSocketConnect()
    {




        byte[] bytes = new byte[1024];

      
        try
        {
            
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(IpAddress), port);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);

                Debug.Log("Socket connected to {0}"+ sender.RemoteEndPoint.ToString());

                Message.text = "Server Online";
                BG.SetActive(false);
                LoadedSystem.SetActive(true);
                string data = "";
                if (DataManager.Instance != null
                         && DataManager.Instance.GetUserId() != null
                         && DataManager.Instance.GetUserId() != ""
                         && DataManager.Instance.GetUserId() != "USERID"
                         && DataManager.Instance.GetUserAccessToken() != null
                         && DataManager.Instance.GetUserAccessToken() != ""
                         && DataManager.Instance.GetUserAccessToken() != "USERACCESSTOKEN")
                {


                    // This user has already login before and we have already set the users saved pref

                    if (DataManager.Instance.GetUserState() == "0" || DataManager.Instance.GetUserState() == "USERSTATE")
                    {


                        DataManager.Instance.SetUserState("0");
                        data = "|SOCKETTYPE|" + "Awake_" + blowfishkey +
                            "|USERACCESSTOKEN|" + DataManager.Instance.GetUserAccessToken() +
                            "|USERID|" + DataManager.Instance.GetUserId() +
                            "|LOGINSTATUS|" + DataManager.Instance.GetUserState();
                    }
                    else if(DataManager.Instance.GetUserState() == "1")
                    {
                        data = "|SOCKETTYPE|" + "Loggingin_" + blowfishkey +
                            "|USERACCESSTOKEN|" + DataManager.Instance.GetUserAccessToken() +
                            "|USERID|" + DataManager.Instance.GetUserId() +
                            "|LOGINSTATUS|" + DataManager.Instance.GetUserState();
                    }
                    else if (DataManager.Instance.GetUserState() == "2")
                    {
                        data = "|SOCKETTYPE|" + "Logged_" + blowfishkey +
                            "|USERACCESSTOKEN|" + DataManager.Instance.GetUserAccessToken() +
                            "|USERID|" + DataManager.Instance.GetUserId() +
                            "|LOGINSTATUS|" + DataManager.Instance.GetUserState();
                    }




                }
                else
                {
                    data = "|SOCKETTYPE|" + "Awake_NewUser_" + blowfishkey;
                }
                SendData(sender, data);
                RecivedData(sender, bytes);
               

            }
            catch (ArgumentNullException ane)
            {
                Debug.Log("ArgumentNullException : {0}"+ ane.ToString());
            }
            catch (SocketException se)
            {
                Debug.Log("SocketException : {0}"+ se.ToString());
                Message.text = "Server Offline";
                BG.SetActive(true);
                LoadedSystem.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log("Unexpected exception : {0}"+ e.ToString());
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
       

       

    }
    private void RecivedData(Socket sender, byte[] bytes)
    {
        
        int bytesRec = sender.Receive(bytes);
        
        string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        Debug.Log("WE GOT: " + data);
        string[] aData = data.Split('|');
        for (int i = 0; i < aData.Length - 1; i++)
        {
            if (aData[i] == "USERACCESSTOKEN")
            {

                Debug.Log("USERACCESSTOKEN: " + aData[i + 1]);
            }
            if (aData[i] == "USERID")
            {

                Debug.Log("USERID: " + aData[i + 1]);
            }
            if (aData[i] == "LOGINSTATUS")
            {
                if(aData[i + 1] == "0")
                {
                   // DataManager.Instance.SetUserState("1");
                }
                Debug.Log("LOGINSTATUS: " + aData[i + 1]);
            }
           
        }

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
       

    }

    private void SendData(Socket sender , string data)
    {
        
        byte[] msg = Encoding.ASCII.GetBytes(data);

         
        sender.Send(msg);
    }

   
    private void Start()
    {
     ServerSocketConnect();
    }

  


   

   

  
    private void CheckStatusOfClient(Socket sender,byte[] bytes)
    {
        


    

                LoadingManager.Instance.FadeOut();
                
        



    }

}
