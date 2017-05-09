using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Collections;
//using System.Threading;

public class ServerStatusManager : MonoBehaviour
{

    private static ServerStatusManager instance;
    public static ServerStatusManager Instance { get { return instance; } }


    public Text Message;
    public GameObject ServerStatusContainor;
    public string IpAddress = "127.0.0.1";
    public int port = 3000;
    private string blowfishkey = "c2VydmVyIGNvbm5lY3Rpb24gdHlwZQ==";

    private string data = "";
    private Socket sender;
    private IPEndPoint remoteEP;







    private bool ServerStatus = false;

    private void Awake()
    {
        instance = this;
    }



    private void Disconnected(Socket sender)
    {
        Debug.Log("DISCONNECTING CLIENTS SOCKET NOW");
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }

    private void ServerSocketConnect()
    {




        byte[] bytes = new byte[1024];


        try
        {

            remoteEP = new IPEndPoint(IPAddress.Parse(IpAddress), port);

            // Create a TCP/IP  socket.  
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);

                Debug.Log("Socket connected to {0}" + sender.RemoteEndPoint.ToString());



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
                    else if (DataManager.Instance.GetUserState() == "1")
                    {
                        data = "|SOCKETTYPE|" + "Loggingin_" + blowfishkey +
                            "|USERACCESSTOKEN|" + DataManager.Instance.GetUserAccessToken() +
                            "|USERID|" + DataManager.Instance.GetUserId() +
                            "|LOGINSTATUS|" + DataManager.Instance.GetUserState()
                            + "|ANDROIDDEVICEID|" + AndroidManager.Instance.GetAndroidDeviceID()
                            + "|CLIENTSIPADDRESS|" + GetIPAddress()
                            + "|USERCREDITS|" + DataManager.Instance.GetUserCredits()
                            + "|USERGPSX|" + DataManager.Instance.GetUserGpsX()
                            + "|USERGPSY|" + DataManager.Instance.GetUserGpsY()
                            + "|USERGPSZ|" + DataManager.Instance.GetUserGpsZ()
                            + "|USERFIRSTNAME|" + DataManager.Instance.GetUserFirstName()
                            + "|USERLASTNAME|" + DataManager.Instance.GetUserLastName()
                            + "|USERNAME|" + DataManager.Instance.GetUserName()
                            + "|USERPIC|" + DataManager.Instance.GetUserPic();
                    }
                    else if (DataManager.Instance.GetUserState() == "2")
                    {
                        data = "|SOCKETTYPE|" + "Logged_" + blowfishkey +
                            "|USERACCESSTOKEN|" + DataManager.Instance.GetUserAccessToken() +
                            "|USERID|" + DataManager.Instance.GetUserId() +
                            "|LOGINSTATUS|" + DataManager.Instance.GetUserState()
                            + "|ANDROIDDEVICEID|" + AndroidManager.Instance.GetAndroidDeviceID()
                            + "|CLIENTSIPADDRESS|" + GetIPAddress()
                            + "|USERCREDITS|" + DataManager.Instance.GetUserCredits()
                            + "|USERGPSX|" + DataManager.Instance.GetUserGpsX()
                            + "|USERGPSY|" + DataManager.Instance.GetUserGpsY()
                            + "|USERGPSZ|" + DataManager.Instance.GetUserGpsZ()
                            + "|USERFIRSTNAME|" + DataManager.Instance.GetUserFirstName()
                            + "|USERLASTNAME|" + DataManager.Instance.GetUserLastName()
                            + "|USERNAME|" + DataManager.Instance.GetUserName()
                            + "|USERPIC|" + DataManager.Instance.GetUserPic();
                    }




                }
                else
                {


                    PrepNewClientData(sender, bytes);

                    return;

                }
                SendData(sender, data);
                RecivedData(sender, bytes);


            }
            catch (ArgumentNullException ane)
            {
                Debug.Log("ArgumentNullException : {0}" + ane.ToString());
            }
            catch (SocketException se)
            {
                Debug.Log("SocketException : {0}" + se.ToString());
                if (AndroidManager.Instance != null)
                {
                    Message.text = "Server Offline: " + AndroidManager.Instance.GetAndroidDeviceID();
                }

            }
            catch (Exception e)
            {
                Debug.Log("Unexpected exception : {0}" + e.ToString());
            }


        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }




    }




    private bool CheckIfLoadedImage = false;
    private void RecivedData(Socket sender, byte[] bytes)
    {

        try
        {
            if (bytes.Length > 0)
            {
                int bytesRec = sender.Receive(bytes);

                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Debug.Log("WE GOT: " + data);
                string[] aData = data.Split('|');
                for (int i = 0; i < aData.Length - 1; i++)
                {

                    if (aData[i] == "NEWCLIENTWATINGLOGIN")
                    {
                        Debug.Log("NEWCLIENTWATINGLOGIN: ");

                        PrepNewClientData(sender, bytes);

                    }

                    if (aData[i] == "ANDROIDDEVICEID")
                    {

                        Debug.Log("ANDROIDDEVICEID: " + aData[i + 1]);
                    }
                    if (aData[i] == "CLIENTSIPADDRESS")
                    {

                        Debug.Log("CLIENTSIPADDRESS: " + aData[i + 1]);
                    }
                    if (aData[i] == "CLIENTSIPADDRESS")
                    {

                        Debug.Log("CLIENTSIPADDRESS: " + aData[i + 1]);
                    }

                    if (aData[i] == "ID")
                    {

                        DataManager.Instance.SetId(aData[i + 1]);
                        Debug.Log("ID: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERID")
                    {
                        DataManager.Instance.SetUserId(aData[i + 1]);
                        Debug.Log("USERID: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERNAME")
                    {
                        DataManager.Instance.SetUserName(aData[i + 1]);
                        Debug.Log("USERNAME: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERPIC")
                    {

                        Debug.Log("USERPIC: " + aData[i + 1]);
                        if (aData[i + 1] == "USERFIRSTNAME")
                        {
                            Debug.Log("USERPIC: WTF DATA " + aData[i + 1]);
                            string userPicture = "https://graph.facebook.com/" + DataManager.Instance.GetUserId() + "/picture?width=200";
                            // DataManager.Instance.new2dpicture(DataManager.Instance.UserImagePic, userPicture);
                            DataManager.Instance.SetUserPic(userPicture);
                        }
                        else
                        {
                            if (CheckIfLoadedImage == false)
                            {
                                DataManager.Instance.new2dpicture(DataManager.Instance.UserImagePic, aData[i + 1]);
                                CheckIfLoadedImage = true;
                            }
                        }

                    }
                    if (aData[i] == "USERFIRSTNAME")
                    {
                        DataManager.Instance.SetUserFirstName(aData[i + 1]);
                        Debug.Log("USERFIRSTNAME: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERLASTNAME")
                    {
                        DataManager.Instance.SetUserLastName(aData[i + 1]);
                        Debug.Log("USERLASTNAME: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERACCESSTOKEN")
                    {
                        DataManager.Instance.SetUserAccessToken(aData[i + 1]);
                        Debug.Log("USERACCESSTOKEN: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERSTATE")
                    {
                        DataManager.Instance.SetUserState(aData[i + 1]);
                        Debug.Log("USERSTATE: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERACCESS")
                    {
                        DataManager.Instance.SetUserAccess(aData[i + 1]);
                        Debug.Log("USERACCESS: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERCREDITS")
                    {
                        DataManager.Instance.SetUserCredits(aData[i + 1]);
                        Debug.Log("USERCREDITS: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERLEVEL")
                    {
                        DataManager.Instance.SetUserLevel(aData[i + 1]);
                        Debug.Log("USERLEVEL: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERMANA")
                    {
                        DataManager.Instance.SetUserMana(aData[i + 1]);
                        Debug.Log("USERMANA: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERHEALTH")
                    {
                        DataManager.Instance.SetUserHealth(aData[i + 1]);
                        Debug.Log("USERHEALTH: " + aData[i + 1]);
                    }
                    if (aData[i] == "USEREXP")
                    {
                        DataManager.Instance.SetUserExp(aData[i + 1]);
                        Debug.Log("USEREXP: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERXPOS")
                    {

                        Debug.Log("USERXPOS: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERYPOS")
                    {

                        Debug.Log("USERYPOS: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERZPOS")
                    {

                        Debug.Log("USERZPOS: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERXROT")
                    {

                        Debug.Log("USERXROT: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERYROT")
                    {

                        Debug.Log("USERYROT: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERZROT")
                    {

                        Debug.Log("USERZROT: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERGPSX")
                    {

                        Debug.Log("USERGPSX: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERGPSY")
                    {

                        Debug.Log("USERGPSY: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERGPSZ")
                    {

                        Debug.Log("USERGPSZ: " + aData[i + 1]);
                    }
                    if (aData[i] == "USERFIRSTTIMELOGIN")
                    {

                        Debug.Log("USERFIRSTTIMELOGIN: " + aData[i + 1]);
                    }


                }
            }
            Disconnected(sender);
        }
        catch (SocketException Se)
        {
            Debug.Log("ERROR RECIVING SOCKET " + Se.Message);
        }
        catch (Exception Se)
        {
            Debug.Log("ERROR RECIVING " + Se.Message);
        }





    }

    string ClientsIPAddress = "";
    public string GetIPAddress()
    {

        IPHostEntry Host = default(IPHostEntry);
        string Hostname = null;
        Hostname = Environment.MachineName;
        Host = Dns.GetHostEntry(Hostname);
        foreach (IPAddress IP in Host.AddressList)
        {

            if (IP.AddressFamily == AddressFamily.InterNetwork)
            {
                ClientsIPAddress = Convert.ToString(IP);
            }
        }
        return ClientsIPAddress;

    }

    private void PrepNewClientData(Socket sender, byte[] bytes)
    {

        if (AndroidManager.Instance != null && DataManager.Instance != null)
        {
            Debug.Log("PrepNewClientData() WE SENT THE DATA ");
            // we sent the current information we have to send to the server 

            data = "|SOCKETTYPE|" + "Awake_NewUser_" + blowfishkey
                + "|ANDROIDDEVICEID|" + AndroidManager.Instance.GetAndroidDeviceID()
                + "|CLIENTSIPADDRESS|" + GetIPAddress()
                + "|USERCREDITS|" + DataManager.Instance.GetUserCredits()
                + "|USERGPSX|" + DataManager.Instance.GetUserGpsX()
                + "|USERGPSY|" + DataManager.Instance.GetUserGpsY()
                + "|USERGPSZ|" + DataManager.Instance.GetUserGpsZ();


            SendData(sender, data);
            RecivedData(sender, bytes);
        }
        else
        {
            Debug.Log("TRYING TO SEND SHIT");

        }
    }

    private void SendData(Socket sender, string data)
    {

        byte[] msg = Encoding.ASCII.GetBytes(data);


        sender.Send(msg);

    }

    private void Update()
    {
        ServerSocketConnect();
    }

    private void Start()
    {
        //ServerSocketConnect();
    }



}
