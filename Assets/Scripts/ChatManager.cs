using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System;

public class ChatManager : MonoBehaviour {

    public string UserID = "1";
    public string UserName = "username";
    public string UserPic = "somepic";
    public InputField TextInput;
    public Text DebugMessage;
    public GameObject MessagePanel;
    public GameObject SpawnMessageList;



    public void OnSubmitBtn()
    {

        if(TextInput.text == "" || TextInput.text.Length < 1)
        {
            Debug.Log("Empty Return");
            DebugMessage.text = "Can Not Send Empty Message!";
            return;
        }
        Debug.Log("MY USER ID = "+ UserID + " MY USERNAME == "+ UserName);

        SendDataToServer(UserID, TextInput.text,  "0");
    }

    private void Update()
    {
        PostMessageData();
    }

    private void PostMessageData()
    {
        if (TextInput.text == "" || TextInput.text.Length < 1)
        {

            return;
        }
       // Debug.Log("TEXT INPUT: = " + TextInput.text);
        DebugMessage.text = TextInput.text;
    }
    int test = 0;
    private void SendDataToServer(string _FromUserID, string _Message, string _ToUserID )
    {
        Debug.Log("FROM WHO? "+ _FromUserID + " SAID WHAT? "+ _Message + " TO WHO? " + _ToUserID);
        
        ServerConnect("|FROMUSERID|"+_FromUserID+"|THEMESSAGE|" + _Message+"|TOUSERID|" + _ToUserID+"|CONNECTIONTYPE|"+test);

    }

    private static int port = 3000;
    private static string IpAddress = "10.0.0.7";

    private void ServerConnect(string mess)
    {
        Socket SocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(IpAddress), port);
        SocketListener.Connect(ipEnd);

        byte[] Buffer = new byte[SocketListener.SendBufferSize];
        int ReadBytes = 0;
        //do
       // {
            
            SocketListener.Send(System.Text.Encoding.UTF8.GetBytes(mess));
        // byte[] pbd = new byte[4];
        // SocketListener.Receive(pbd);


        // } while (mess.Length > 0);
        if (Buffer.Length > 0)
        {
            ReadBytes = SocketListener.Receive(Buffer);

            byte[] rData = new byte[ReadBytes];
            Array.Copy(Buffer, rData, ReadBytes);
            Debug.Log("We Recived: " + System.Text.Encoding.UTF8.GetString(rData));


            // some how we need to get the data back into the client
            //something like a new message
            //Newmessage("mess", userpic ,null)
            CreateNewMessage(System.Text.Encoding.UTF8.GetString(rData));
        }
        
        SocketListener.Close();



    }


    private void CreateNewMessage(string mess)
    {

        // we know we want to instantionate the prefab 
        //MessagePanel
        //SpawnMessageList

       

      

            GameObject NewMessage = (GameObject)Instantiate(MessagePanel);
        Debug.Log(NewMessage.transform.GetChild(0).name); // UserPic
        Debug.Log(NewMessage.transform.GetChild(1).name); // UserName
        Debug.Log(NewMessage.transform.GetChild(2).name); // PostMessage
        Debug.Log(NewMessage.transform.GetChild(3).name); // DeleteBtn

        Text UserName = NewMessage.transform.GetChild(1).GetComponent<Text>();
        Text PostMessage = NewMessage.transform.GetChild(2).GetComponent<Text>();


        string[] aData = mess.Split('|');
        for (int i = 0; i < aData.Length - 1; i++)
        {
            if (aData[i] == "FROMUSERID")
            {
                UserName.text = aData[i + 1];
                Debug.Log("FROMUSERID: " + aData[i + 1]);
            }
            if (aData[i] == "THEMESSAGE")
            {
                PostMessage.text = aData[i + 1];
                Debug.Log("THEMESSAGE: " + aData[i + 1]);
            }
            if (aData[i] == "TOUSERID")
            {

                Debug.Log("TOUSERID: " + aData[i + 1]);
            }
            if (aData[i] == "CONNECTIONTYPE")
            {
                if (test == 1)
                {
                    // we sent to server as 2
                    test = 2;
                }
                if (test == 0)
                {
                   // we sent to server as 1
                    test = 1;
                }
               
                Debug.Log("CONNECTIONTYPE: " + aData[i + 1]);
                
            }
        }
        
      





        NewMessage.transform.SetParent(SpawnMessageList.transform);
        NewMessage.transform.localScale = Vector3.one;

    }
}
