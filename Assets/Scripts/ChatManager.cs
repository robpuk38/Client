
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChatManager : MonoBehaviour {

    private static ChatManager instance;
    public static ChatManager Instance { get { return instance; } }
    public string FromUserID = "1";
    public string ToUserID = "0";
    private string Message = "";
   
    public string UserName = "username";
    public string UserPic = "somepic";
    public InputField TextInput;
    public Text DebugMessage;
    public GameObject MessagePanel;
    public GameObject SpawnMessageList;
    public GameObject ChatSystemContainor;
    public GameObject ChatMenu;
    public Sprite Chaton;
    public Sprite Chatoff;
    private Sprite CurrentUsersPic;
    public bool ShowChat { get; set; }

    private void Awake()
    {
        instance = this;
        ChatSystemContainor.SetActive(false);
    }


    private void GetCurrentUserData()
    {
        if(DataManager.Instance != null)
        {
            FromUserID = DataManager.Instance.GetUserId();
            UserName = DataManager.Instance.GetUserName();
            // if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
            //{
            //   Debug.Log("WhAT DID WE CLICK: " + EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name);
            // }
            CurrentUsersPic = DataManager.Instance.UserImagePic.GetComponent<Image>().sprite;
        }
    }

    public void ToggleChatBtn()
    {
        if(ChatSystemContainor.activeSelf == false)
        {
            ShowChat = true;
            ServerStatusManager.Instance.ShowMenu = false;
            ChatSystemContainor.SetActive(true);
            ChatMenu.GetComponent<Image>().sprite = Chaton;
        }
        else
        {
            ShowChat = false;
            ChatSystemContainor.SetActive(false);
            ChatMenu.GetComponent<Image>().sprite = Chatoff;
        }
    }

    public void OnSubmitBtn()
    {

        if(TextInput.text == "" || TextInput.text.Length < 1 )
        {
            Debug.Log("Empty Return");
            DebugMessage.text = "Can Not Send Empty Message!";
            return;
        }
        if (TextInput.text == "" || TextInput.text.Length > 120)
        {
            Debug.Log("To long Return");
            DebugMessage.text = "Messsage Was To Long Only Use 120 Characters Max!";
            return;
        }
        Debug.Log("MY USER ID = "+ FromUserID + " MY USERNAME == "+ UserName);

        SendDataToServer(FromUserID, UserName, CurrentUsersPic, TextInput.text,  ToUserID);
    }

    private void Update()
    {
        PostMessageData();

        if(ShowChat == false)
        {
            ChatSystemContainor.SetActive(false);
            ChatMenu.GetComponent<Image>().sprite = Chatoff;
        }
        GetCurrentUserData();
    }

    private void PostMessageData()
    {
        if (TextInput.text == "" || TextInput.text.Length < 1)
        {

            return;
        }
       // Debug.Log("TEXT INPUT: = " + TextInput.text);
        DebugMessage.text = TextInput.text;
        Message = DebugMessage.text;
    }
   
    private void SendDataToServer(string _FromUserID,string Username, Sprite CurrentUserspic, string _Message, string _ToUserID )
    {
        Debug.Log("FROM WHO? "+ _FromUserID + " SAID WHAT? "+ _Message + " TO WHO? " + _ToUserID);

        string data = Construct.FROMUSERID + _FromUserID + Construct.USERNAME + Username + Construct.THEMESSAGE + _Message + Construct.TOUSERID + _ToUserID;
        SetFromUserId(_FromUserID);
        SetToUserId(_ToUserID);
        SetMessage(_Message);
        CreateNewMessage(data, CurrentUserspic);

        ServerStatusManager.Instance.SendNewDataType(Construct.ONNEWMESSAGE);
    }


    public void SetFromUserId(string set)
    {
        FromUserID = set;
    }

    public string GetFromUserId()
    {
        return FromUserID;
    }

    public void SetToUserId(string set)
    {
        ToUserID = set;
    }

    public string GetToUserId()
    {
        return ToUserID;
    }

    public void SetMessage(string set)
    {
        Message = set;
    }

    public string GetMessage()
    {
        return Message;
    }

    private void CreateNewMessage(string mess, Sprite Currentuserspic)
    {

        GameObject NewMessage = (GameObject)Instantiate(MessagePanel);
        Debug.Log(NewMessage.transform.GetChild(0).name); // UserPic
        Debug.Log(NewMessage.transform.GetChild(1).name); // UserName
        Debug.Log(NewMessage.transform.GetChild(2).name); // PostMessage
        Debug.Log(NewMessage.transform.GetChild(3).name); // DeleteBtn

        Text UserName = NewMessage.transform.GetChild(1).GetComponent<Text>();
        Text PostMessage = NewMessage.transform.GetChild(2).GetComponent<Text>();
        Image MyUserPic = NewMessage.transform.GetChild(0).GetComponent<Image>();
        MyUserPic.sprite = Currentuserspic;

        string[] aData = mess.Split('|');
        for (int i = 0; i < aData.Length - 1; i++)
        {
            if (aData[i] == Construct._FROMUSERID)
            {
                Debug.Log(Construct._FROMUSERID+": " + aData[i + 1]);
            }
            if (aData[i] == Construct._THEMESSAGE)
            {
                PostMessage.text = aData[i + 1];
                Debug.Log(Construct._THEMESSAGE+": " + aData[i + 1]);
            }
            if (aData[i] == Construct._TOUSERID)
            {

                Debug.Log(Construct._TOUSERID+": " + aData[i + 1]);
            }

            if (aData[i] == Construct._USERNAME)
            {
                UserName.text = aData[i + 1];
                Debug.Log(Construct._USERNAME+": " + aData[i + 1]);
            }

        }
        
      





        NewMessage.transform.SetParent(SpawnMessageList.transform);
        NewMessage.transform.localScale = Vector3.one;
        TextInput.text = "";

    }

    public void CreateFomeMessage(string data)
    {
        //Debug.Log(data);
        string FromUserId = "";
        string FromUserName = "";
        string Message = "";
        string FromUserPic = "";
        string[] aData = data.Split('|');
        for (int i = 0; i < aData.Length - 1; i++)
        {
            if (aData[i] == Construct._FROMUSERID)
            {
                FromUserId = aData[i + 1];
                Debug.Log(Construct._FROMUSERID + ": " + aData[i + 1]);
            }

            if (aData[i] == Construct._FROMUSERPIC)
            {
                FromUserPic = aData[i + 1];
                Debug.Log(Construct._FROMUSERPIC + ": " + aData[i + 1]);

                
            }
            if (aData[i] == Construct._THEMESSAGE)
            {
                Message = aData[i + 1];
                Debug.Log(Construct._THEMESSAGE + ": " + aData[i + 1]);
            }
            if (aData[i] == Construct._TOUSERID)
            {

                Debug.Log(Construct._TOUSERID + ": " + aData[i + 1]);
            }

            if (aData[i] == Construct._FROMUSERNAME)
            {
                FromUserName = aData[i + 1];
                Debug.Log(Construct._FROMUSERNAME + ": " + aData[i + 1]);
            }

        }
        if (FromUserId != Construct._NULL && DataManager.Instance.GetUserId() != FromUserId)
        { 
        GameObject NewMessage = (GameObject)Instantiate(MessagePanel);
        Debug.Log(NewMessage.transform.GetChild(0).name); // UserPic
        Debug.Log(NewMessage.transform.GetChild(1).name); // UserName
        Debug.Log(NewMessage.transform.GetChild(2).name); // PostMessage
        Debug.Log(NewMessage.transform.GetChild(3).name); // DeleteBtn

        Text UserName = NewMessage.transform.GetChild(1).GetComponent<Text>();
        Text PostMessage = NewMessage.transform.GetChild(2).GetComponent<Text>();
        Transform UserPic = NewMessage.transform.GetChild(0).GetComponent<Transform>();

           


        for (int i = 0; i < aData.Length - 1; i++)
        {
            if (aData[i] == Construct._FROMUSERID)
            {
                Debug.Log(Construct._FROMUSERID + ": " + aData[i + 1]);
            }

            if (aData[i] == Construct._FROMUSERPIC)
            {
                //UserName.text = aData[i + 1];
                Debug.Log(Construct._FROMUSERPIC + ": " + aData[i + 1]);
                    DataManager.Instance.new2dpicture(UserPic.gameObject, FromUserPic);
                }
            if (aData[i] == Construct._THEMESSAGE)
            {

                    if (PostMessage.text == Message)
                    {
                        return;
                    }
                    PostMessage.text = aData[i + 1];
                    Debug.Log(Construct._THEMESSAGE + ": " + aData[i + 1]);
            }
            if (aData[i] == Construct._TOUSERID)
            {

                Debug.Log(Construct._TOUSERID + ": " + aData[i + 1]);
            }

            if (aData[i] == Construct._FROMUSERNAME)
            {
                UserName.text = aData[i + 1];
                Debug.Log(Construct._FROMUSERNAME + ": " + aData[i + 1]);
            }

        }






        
            NewMessage.transform.SetParent(SpawnMessageList.transform);
            NewMessage.transform.localScale = Vector3.one;
        }
        

    }
}
