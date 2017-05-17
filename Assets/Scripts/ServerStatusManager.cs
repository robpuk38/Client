using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;


public class ServerStatusManager : MonoBehaviour
{

    private static ServerStatusManager instance;
    public static ServerStatusManager Instance { get { return instance; } }


    public Text Message;
    public GameObject ServerStatusContainor;
    public GameObject MenuSwitchContainor;
    public GameObject ToggleMenu;
    public Sprite Menuon;
    public Sprite Menuoff;
   // public string IpAddress = "127.0.0.1";
    //public int port = 3000;
    

    private string data = "";
    private Socket sender;
    private IPEndPoint remoteEP;

   

    private void Awake()
    {
        instance = this;
        
        MenuSwitchContainor.SetActive(false);
    }

    public void ToggleMenuBtn()
    {
        if (ServerStatusContainor.activeSelf == false)
        {
            ShowMenu = true;
            ChatManager.Instance.ShowChat = false;
            ServerStatusContainor.SetActive(true);
            ToggleMenu.GetComponent<Image>().sprite = Menuon;
        }
        else
        {
            ShowMenu = false;
            ServerStatusContainor.SetActive(false);
            ToggleMenu.GetComponent<Image>().sprite = Menuoff;
        }
    }

    private void Disconnected(Socket sender)
    {
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }

   
    public void SendNewDataType(string type)
    {
        try
        {
            remoteEP = new IPEndPoint(IPAddress.Parse(SystemConfig.Instance.ServerIpAddress), SystemConfig.Instance.ServerPortNumber);

            // Create a TCP/IP  socket.  
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);

                if(type == Construct.ONAWAKE)
                {
                    SendData(sender, ONAWAKE());
                    
                }
                if (type == Construct.ONADS)
                {
                    SendData(sender, ONADS());

                }
                if (type == Construct.ONLOGIN)
                {
                    SendData(sender, ONLOGIN());

                }
                if (type == Construct.ONLOGOUT)
                {
                    SendData(sender, ONLOGOUT());

                }
                if (type == Construct.ONNEWMESSAGE)
                {
                    SendData(sender, ONNEWMESSAGE());

                }
                byte[] bytes = new Byte[sender.SendBufferSize];
                RecivedData(sender, bytes);
                //SendData(sender, data);
                //
            }
            catch (ArgumentNullException ane)
            {
                //Debug.Log("ArgumentNullException : {0}" + ane.ToString());
            }
            catch (SocketException se)
            {
                //Debug.Log("SocketException : {0}" + se.ToString());


            }
            catch (Exception e)
            {
                //Debug.Log("Unexpected exception : {0}" + e.ToString());
            }


        }
        catch (Exception e)
        {
            //Debug.Log(e.ToString());
        }
    }
    public bool ShowMenu { get; set; }
    public bool FullLogIn { get; set; }
    public bool CheckIfLoadedImage { get; set; }
    public bool CheckAndSendOnce { get; set; }

    private void ClientUpdateDataManager(string data)
    {
        string[] aData = data.Split('|');
        for (int i = 0; i < aData.Length - 1; i++)
        {

            if (aData[i] == Construct._USERSTATE)
            {

                //Debug.Log(Construct._USERSTATE + " " + aData[i + 1]);

                if (aData[i + 1] == Construct._TWO && FacebookManager.Instance.FacebookLogoutBtn.activeSelf == true && DataManager.Instance.GetUserState() == Construct._TWO)
                {
                    FullLogIn = true;
                    // THE CLIENT IS LOGIN WE RETURNED A 1 SO LETS SEND BACK A 2
                    // Debug.Log("ARE WE LOGGIN SUCCESSFLULLY CHANGE STATE TO 2:= " + aData[i + 1]);
                    DataManager.Instance.SetUserState(Construct._THREE);
                }

                if (aData[i + 1] == Construct._ONE && FacebookManager.Instance.FacebookLogoutBtn.activeSelf == true && DataManager.Instance.GetUserState() == Construct._ONE)
                {
                    // THE CLIENT IS LOGIN WE RETURNED A 1 SO LETS SEND BACK A 2
                  //  Debug.Log("ARE WE LOGGIN SUCCESSFLULLY CHANGE STATE TO 1:= " + aData[i + 1]);
                    DataManager.Instance.SetUserState(Construct._TWO);
                }

                if (aData[i + 1] == Construct._ZERO && FacebookManager.Instance.FacebookLogoutBtn.activeSelf == true)
                {


                  //  Debug.Log(Construct._USERSTATE + " OUR USER STATE IS A BIG NOTHING SO WE ARE NOT LOGIN " + aData[i + 1]);
                    FacebookManager.Instance.LoggedMessage.text = "LOGOUT";
                    DataManager.Instance.SetUserState(Construct._ZERO);
                    //ServerStatusManager.Instance.SendNewDataType(Construct.ONLOGOUT);
                    FacebookManager.Instance.FacebookLoginBtn.SetActive(true);
                    FacebookManager.Instance.FacebookLogoutBtn.SetActive(false);
                    DataManager.Instance.UserName.text = Construct._USERGUEST;
                    // DataManager.Instance.SetUserId(Construct._USERID);
                    // DataManager.Instance.SetUserName(Construct._USERGUEST);
                    // DataManager.Instance.SetUserPic(Construct._USERPIC);
                    FullLogIn = false;
                    DataManager.Instance.UserImagePic.GetComponent<Image>().sprite = FacebookManager.Instance.User;
                    CheckIfLoadedImage = false;
                    CheckAndSendOnce = false;
                    FacebookManager.Instance.HasLogout = true;
                    Debug.Log("ARE WE ARE LOGOUT " + FacebookManager.Instance.HasLogout);

                }
                
            }

            if (aData[i] == Construct._ID)
            {

                //Debug.Log(Construct._ID + " " + aData[i + 1]);
                DataManager.Instance.SetId(aData[i + 1]);
            }

            if (aData[i] == Construct._USERACCESS)
            {
                
                //Debug.Log(Construct._USERACCESS + " " + aData[i + 1]);
                DataManager.Instance.SetUserAccess(aData[i + 1]);
            }

            if (aData[i] == Construct._USERACCESSTOKEN)
            {
                if (DataManager.Instance.GetUserAccessToken() != aData[i + 1])
                {
                   // Debug.Log(" THIS ACCESS TOKEN DOES NOT MATCH WHAT IS STORED " + aData[i + 1]);
                    DataManager.Instance.SetUserAccessToken(aData[i + 1]);
                }
                else
                {
                   // Debug.Log("THIS ACCESSTOKEN "+Construct._USERACCESSTOKEN + " " + aData[i + 1]);
                   // DataManager.Instance.SetUserAccessToken(aData[i + 1]);
                }
                    
            }

            if (aData[i] == Construct._USERACTIVATION)
            {

                //Debug.Log(Construct._USERACTIVATION + " " + aData[i + 1]);
                DataManager.Instance.SetUserActivation(aData[i + 1]);
            }

            if (aData[i] == Construct._USERCREDITS)
            {

                //Debug.Log(Construct._USERCREDITS + " " + aData[i + 1]);

                if(aData[i + 1] == Construct._NEGITIVEONE)
                {
                    // THIS IS CLIENT HAS LOGIN WITH A DEVICE THAT IS KNOWEN
                    // AND WE ARE RESETING THE CREDITS TO A -1 SO WE KNOW THEY ARE A NEW MEMBER
                    // AND THEY HAVE NOT MET THE REQURMENTS TO JOIN THE SERVER YET
                    // SO LETS LOGOUT THIS USER AND SET THE CREDITS TO 0 FOR THEM SO THEY HAVE TO MEET THE REQUIREMENTS 
                    DataManager.Instance.SetUserCredits(Construct._ZERO);
                    FacebookManager.Instance.FacebookLogout();
                }
                else
                {
                    DataManager.Instance.SetUserCredits(aData[i + 1]);
                }
                
            }

            if (aData[i] == Construct._USERDEVICEID)
            {

                //Debug.Log(Construct._USERDEVICEID + " " + aData[i + 1]);

                if (aData[i + 1] != DataManager.Instance.GetUserDeviceId())
                {
                    //Debug.Log("THIS DEVICE IS NOT MATCHING WITH CURRENT DEVICE");
                    FacebookManager.Instance.FacebookLogout();
                }
                else
                {
                    if (aData[i + 1] == Construct._SWITCHED_ACCOUNTS)
                    {

                    }
                    else
                    {
                        DataManager.Instance.SetUserDeviceId(aData[i + 1]);
                    }
                }
            }

            if (aData[i] == Construct._USEREXP)
            {

                //Debug.Log(Construct._USEREXP + " " + aData[i + 1]);
                DataManager.Instance.SetUserExp(aData[i + 1]);
            }

            if (aData[i] == Construct._USERFIRSTNAME)
            {

                //Debug.Log(Construct._USERFIRSTNAME + " " + aData[i + 1]);
                DataManager.Instance.SetUserFirstName(aData[i + 1]);
            }

            if (aData[i] == Construct._USERFIRSTTIMELOGIN)
            {

                //Debug.Log(Construct._USERFIRSTTIMELOGIN + " " + aData[i + 1]);
                DataManager.Instance.SetUserFirstTimeLogin(aData[i + 1]);
            }

            if (aData[i] == Construct._USERGPSX)
            {

                //Debug.Log(Construct._USERGPSX + " " + aData[i + 1]);
                DataManager.Instance.SetUserGpsX(aData[i + 1]);
            }

            if (aData[i] == Construct._USERGPSY)
            {

                //Debug.Log(Construct._USERGPSY + " " + aData[i + 1]);
                DataManager.Instance.SetUserGpsY(aData[i + 1]);
            }

            if (aData[i] == Construct._USERGPSZ)
            {

                //Debug.Log(Construct._USERGPSZ + " " + aData[i + 1]);
                DataManager.Instance.SetUserGpsZ(aData[i + 1]);
            }

            if (aData[i] == Construct._USERHEALTH)
            {

                //Debug.Log(Construct._USERHEALTH + " " + aData[i + 1]);
                DataManager.Instance.SetUserHealth(aData[i + 1]);
            }

            if (aData[i] == Construct._USERID)
            {

                //Debug.Log(Construct._USERID + " " + aData[i + 1]);

                if (aData[i + 1] != Construct._USERID && aData[i + 1] != Construct._ZERO && aData[i + 1] != Construct._USERDEVICEID && aData[i + 1] != Construct._USERADSMODTYPE && aData[i + 1] != Construct._USERSTATE && aData[i + 1] != Construct._NULL && aData[i + 1] != Construct._USERACCESSTOKEN && aData[i + 1] != Construct._USERNAME)
                {

                    if (DataManager.Instance.GetUserId() != aData[i + 1])
                    {
                       // Debug.Log("THIS USER ID DOES NOT MATCH " + aData[i + 1]);
                        DataManager.Instance.SetUserId(aData[i + 1]);
                    }
                    //else
                   // {
                        //Debug.Log("THIS USER ID MATCHS " + aData[i + 1]);
                   // }
                        //Debug.Log("WE ARE SETTING THE USERS ID AS THIS RIGHT NOW " + aData[i + 1]);
                       
                }
            }

            if (aData[i] == Construct._USERIPADDRESS)
            {

                //Debug.Log(Construct._USERIPADDRESS + " " + aData[i + 1]);

                if (aData[i + 1] != DataManager.Instance.GetUserIpAddress())
                {
                    //Debug.Log("THIS IP IS NOT MATCHING CURRENT IP");
                    FacebookManager.Instance.FacebookLogout();
                }
                else
                {
                    
                    DataManager.Instance.SetUserIpAddress(aData[i + 1]);
                }
            }

            if (aData[i] == Construct._USERLASTNAME)
            {

                //Debug.Log(Construct._USERLASTNAME + " " + aData[i + 1]);
                DataManager.Instance.SetUserLastName(aData[i + 1]);
            }

            if (aData[i] == Construct._USERLEVEL)
            {

                //Debug.Log(Construct._USERLEVEL + " " + aData[i + 1]);
                DataManager.Instance.SetUserLevel(aData[i + 1]);
            }

            if (aData[i] == Construct._USERMANA)
            {

                //Debug.Log(Construct._USERMANA + " " + aData[i + 1]);
                DataManager.Instance.SetUserMana(aData[i + 1]);
            }

           

            if (aData[i] == Construct._USERPIC)
            {

                //Debug.Log(Construct._USERPIC + " " + aData[i + 1]);
                DataManager.Instance.SetUserPic(aData[i + 1]);
                if (CheckIfLoadedImage == false && aData[i + 1] != Construct._USERPIC && FacebookManager.Instance.HasLogout == false)
                {
                    CheckIfLoadedImage = true;
                    DataManager.Instance.new2dpicture(DataManager.Instance.UserImagePic, DataManager.Instance.GetUserPic());
                }
            }

            

            if (aData[i] == Construct._USERNAME)
            {

                //Debug.Log(Construct._USERNAME + " " + aData[i + 1]);

                if (FacebookManager.Instance.HasLogout == false )
                {
                    //Debug.Log(" WE ARE LOGIN SO STAY LOGGIN  ");
                    DataManager.Instance.SetUserName(aData[i + 1]);
                }
              
            }

            if (aData[i] == Construct._USERXPOS)
            {

                //Debug.Log(Construct._USERXPOS + " " + aData[i + 1]);
                DataManager.Instance.SetUserXPos(aData[i + 1]);
            }

            if (aData[i] == Construct._USERYPOS)
            {

                //Debug.Log(Construct._USERYPOS + " " + aData[i + 1]);
                DataManager.Instance.SetUserYPos(aData[i + 1]);
            }

            if (aData[i] == Construct._USERZPOS)
            {

                //Debug.Log(Construct._USERZPOS + " " + aData[i + 1]);
                DataManager.Instance.SetUserZPos(aData[i + 1]);
            }

            if (aData[i] == Construct._USERXROT)
            {

                //Debug.Log(Construct._USERXROT + " " + aData[i + 1]);
                DataManager.Instance.SetUserXRot(aData[i + 1]);
            }

            if (aData[i] == Construct._USERYROT)
            {

                //Debug.Log(Construct._USERYROT + " " + aData[i + 1]);
                DataManager.Instance.SetUserYRot(aData[i + 1]);
            }

            if (aData[i] == Construct._USERZROT)
            {

                //Debug.Log(Construct._USERZROT + " " + aData[i + 1]);
                DataManager.Instance.SetUserZRot(aData[i + 1]);
            }

            if (aData[i] == Construct._USERADSMODTYPE)
            {

                //Debug.Log(Construct._USERADSMODTYPE + " " + aData[i + 1]);
                DataManager.Instance.SetUserAdsMod(Construct._ZERO);
            }

        }
        //lets check if this client has all the data set to know if they are fully login or not

       // Debug.Log("USERID: "+ DataManager.Instance.GetUserId());
       // Debug.Log("USERNAME: " + DataManager.Instance.GetUserName());
       // Debug.Log("USERPIC: " + DataManager.Instance.GetUserPic());
        //Debug.Log("USERSTATE: " + DataManager.Instance.GetUserState());
        if (DataManager.Instance.GetUserId() != Construct._USERID 
            && DataManager.Instance.GetUserName() != Construct._USERNAME
            && DataManager.Instance.GetUserPic() != Construct._USERPIC
             && DataManager.Instance.GetUserState() != Construct._ZERO
             && CheckAndSendOnce == false && FacebookManager.Instance.HasLogout == false)
        {

          //  Debug.Log("DID WE MAKE IT IN : ");
            CheckAndSendOnce = true;
            FacebookManager.Instance.LoggedMessage.text = "LOGIN";
            FacebookManager.Instance.FacebookLoginBtn.SetActive(false);
            FacebookManager.Instance.FacebookLogoutBtn.SetActive(true);
            //here it is 
            if (DataManager.Instance.GetUserState() == Construct._USERSTATE)
            {
                DataManager.Instance.SetUserState(Construct._ONE);
            }
            FacebookManager.Instance.WelcomeMessage.text = "Welcome, " + DataManager.Instance.GetUserFirstName() + " To " + SystemConfig.Instance.AppName;
            SendNewDataType(Construct.ONLOGIN);
            FacebookManager.Instance.HasLogout = false;
        }
    }


    private void RecivedData(Socket sender, byte[] bytes)
    {
      int bytesRec = sender.Receive(bytes);
      data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
               

      
            ClientUpdateDataManager(data);
            Disconnected(sender);
            

        

        //Debug.Log("UNK: " + data);

        
        

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
                DataManager.Instance.SetUserIpAddress(ClientsIPAddress);
            }
        }
        return ClientsIPAddress;

    }

    private string ONAWAKE()
    {
       
        data = Construct.CONNECTIONTYPE + Construct.ONAWAKE
               + Construct.USERDEVICEID + AndroidManager.Instance.GetAndroidDeviceID()
               + Construct.USERIPADDRESS + GetIPAddress()
               + Construct.USERCREDITS + DataManager.Instance.GetUserCredits()
               + Construct.USERGPSX + DataManager.Instance.GetUserGpsX()
               + Construct.USERGPSY + DataManager.Instance.GetUserGpsY()
               + Construct.USERGPSZ + DataManager.Instance.GetUserGpsZ();
        return data;
    }

    private string ONADS()
    {

        //Debug.Log("WHAT IS MY USERID SAYING?? "+DataManager.Instance.GetUserId());
        if (DataManager.Instance.GetUserDeviceId() == Construct._SWITCHED_ACCOUNTS)
        {


            // the user logout we may be switching accounts so lets clean the temp table
            if(DataManager.Instance.GetUserId() != Construct._USERID)
            {
                DataManager.Instance.SetUserId(Construct._USERID);
            }

            data = Construct.CONNECTIONTYPE + Construct.ONSWITCHEDACCOUNT
              + Construct.USERDEVICEID + AndroidManager.Instance.GetAndroidDeviceID()
              + Construct.USERIPADDRESS + GetIPAddress()
              + Construct.USERCREDITS + Construct._ZERO
              + Construct.USERGPSX + DataManager.Instance.GetUserGpsX()
              + Construct.USERGPSY + DataManager.Instance.GetUserGpsY()
              + Construct.USERGPSZ + DataManager.Instance.GetUserGpsZ()
              + Construct.USERID + Construct._USERID;
             

            return data;
        }


        data = Construct.CONNECTIONTYPE + Construct.ONADS
               + Construct.USERDEVICEID + AndroidManager.Instance.GetAndroidDeviceID()
               + Construct.USERIPADDRESS + GetIPAddress()
               + Construct.USERCREDITS + DataManager.Instance.GetUserCredits() 
               + Construct.USERGPSX + DataManager.Instance.GetUserGpsX()
               + Construct.USERGPSY + DataManager.Instance.GetUserGpsY()
               + Construct.USERGPSZ + DataManager.Instance.GetUserGpsZ()
               + Construct.USERID + DataManager.Instance.GetUserId()
               + Construct.USERADSMODTYPE + DataManager.Instance.GetUserAdsMod()
               + Construct.USERSTATE + DataManager.Instance.GetUserState();
              
        return data;
    }


    private string ONNEWMESSAGE()
    {
        data = Construct.CONNECTIONTYPE + Construct.ONNEWMESSAGE
           + Construct.FROMUSERID + ChatManager.Instance.GetFromUserId()
           + Construct.TOUSERID + ChatManager.Instance.GetToUserId()
           + Construct.THEMESSAGE + ChatManager.Instance.GetMessage();
           return data;
    }

    private string ONLOGIN()
    {

        data = Construct.CONNECTIONTYPE + Construct.ONLOGIN
               + Construct.USERDEVICEID + AndroidManager.Instance.GetAndroidDeviceID()
               + Construct.USERIPADDRESS + GetIPAddress()
               + Construct.USERCREDITS + DataManager.Instance.GetUserCredits()
               + Construct.USERGPSX + DataManager.Instance.GetUserGpsX()
               + Construct.USERGPSY + DataManager.Instance.GetUserGpsY()
               + Construct.USERGPSZ + DataManager.Instance.GetUserGpsZ()
               + Construct.USERFIRSTNAME + DataManager.Instance.GetUserFirstName()
               + Construct.USERID + DataManager.Instance.GetUserId()
               + Construct.USERACCESSTOKEN + DataManager.Instance.GetUserAccessToken()
               + Construct.USERPIC + DataManager.Instance.GetUserPic()
               + Construct.USERNAME + DataManager.Instance.GetUserName()
               + Construct.USERLASTNAME + DataManager.Instance.GetUserLastName()
               + Construct.USERSTATE + DataManager.Instance.GetUserState()
               + Construct.USERACTIVATION + DataManager.Instance.GetUserActivation();
        return data;
    }

    private string ONLOGOUT()
    {

        data = Construct.CONNECTIONTYPE + Construct.ONLOGOUT
               + Construct.USERID + DataManager.Instance.GetUserId()
               + Construct.USERSTATE + DataManager.Instance.GetUserState();
        return data;
    }



    private void SendData(Socket sender, string data)
    {
        byte[] msg = Encoding.ASCII.GetBytes(data);
        sender.Send(msg);
    }

    private int PingServerTime = 0;
    private void Update()
    {
        if(PingServerTime < 200)
        {
            PingServerTime++;
        }
        if(PingServerTime > 199)
        {
            SendNewDataType(Construct.ONADS);
            PingServerTime = 0;
        }
        DataManager.Instance.UserinputCredits.text = DataManager.Instance.UserCredits.text;
       

        if (DataManager.Instance.GetUserState() == Construct._THREE && FullLogIn == true && ShowMenu == false)
        {
           // Debug.Log("IT SEEMS WE ARE FULLY LOGIN");
            ServerStatusContainor.SetActive(false);
            MenuSwitchContainor.SetActive(true);
            ToggleMenu.GetComponent<Image>().sprite = Menuoff;
        }
    }

    

    private void Start()
    {

        SendNewDataType(Construct.ONAWAKE);
        
        //ServerSocketConnect();
        ////Debug.Log("CONTRUCT DATA: "+Contruct.CONNECTIONTYPE);
    }



}
