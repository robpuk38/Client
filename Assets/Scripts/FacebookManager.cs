using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;


public class FacebookManager : MonoBehaviour
{

    private static FacebookManager instance;
    public static FacebookManager Instance { get { return instance; } }
    public GameObject Disable;
    public GameObject FacebookLoginBtn;
    public GameObject FacebookLogoutBtn;
    public Text LoggedMessage;
    public Text WelcomeMessage;
    public Sprite User;
    //public string AppName = "AppName";


    private void Awake()
     {
         instance = this;
   

         //if (!FB.IsInitialized)
         //{
             FB.Init();
        // }

        FacebookLogoutBtn.SetActive(false);
        FacebookLoginBtn.SetActive(true);


    }
     

    private void Update()
    {
        if (DataManager.Instance != null)
        {
            int newcredits = 0;
            int.TryParse(DataManager.Instance.GetUserCredits(), out newcredits);

            if (newcredits > 199)
            {
                Disable.SetActive(false);
            }
            else
            {
                Disable.SetActive(true);

            }


        }
    }
    public void FacebookLogin()
    {
       
        if (DataManager.Instance != null)
        {
            int newcredits = 0;
            int.TryParse(DataManager.Instance.GetUserCredits(), out newcredits);

            if (newcredits > 199)
            {
               // Debug.Log("OK YOU HAVE ENOUGH CREDITS TO JOIN");
                //  if (FB.IsLoggedIn)
                // {
               // PlayerPrefs.DeleteAll();
                   // Debug.Log("We are alreay login");
                    LoginStatusMemory();

                    LoggedMessage.text = "LOGIN";
                    FacebookLoginBtn.SetActive(false);
                    FacebookLogoutBtn.SetActive(true);
                    DataManager.Instance.SetUserState(Construct._ONE);
                    WelcomeMessage.text = "Welcome, " + DataManager.Instance.GetUserFirstName() + " To " + SystemConfig.Instance.AppName;
                    ServerStatusManager.Instance.SendNewDataType(Construct.ONLOGIN);
                    HasLogout = false;

                    return;
               // }
             //   else
               // {
                  //  LoginStatusMemory();
                   // HasLogout = false;
                   // return;
               // }
            }
            else
            {
               // Debug.Log("SEND POP UP TELLING THEM THEY NEED MORE CREDITS ");
                PopUpContationManager.Instance.Toggle();
            }

                
        }
        
    }
    public bool HasLogout { get; set; }
    public void FacebookLogout()
    {
        if (HasLogout == false)
        {
            Debug.Log("YOU HAVE BEEN LOGGED OUT OF FACEBOOK");
            LoggedMessage.text = "LOGOUT";
            DataManager.Instance.SetUserState(Construct._ZERO);
            DataManager.Instance.SetUserCredits(Construct._ZERO);
            DataManager.Instance.SetUserDeviceId(Construct._SWITCHED_ACCOUNTS);
            ServerStatusManager.Instance.SendNewDataType(Construct.ONLOGOUT);
            FacebookLoginBtn.SetActive(true);
            FacebookLogoutBtn.SetActive(false);
            DataManager.Instance.UserName.text = Construct._USERGUEST;
           

            DataManager.Instance.UserImagePic.GetComponent<Image>().sprite = User;
            ServerStatusManager.Instance.CheckIfLoadedImage = false;
            ServerStatusManager.Instance.CheckAndSendOnce = false;
            HasLogout = true;
            if (FB.IsLoggedIn)
            {
                FB.LogOut();
            }
            
        }
    }

    private void LoginStatusMemory()
    {
        if (!FB.IsLoggedIn)
        {
            List<string> perm = new List<string>();
            perm.Add("public_profile");
            FB.LogInWithReadPermissions(permissions: perm, callback: OnLogin);
        }
    }

    private void OnLogin(ILoginResult result)
    {
        if (!result.Cancelled)
        {
            Debug.Log("Successful Login");
            //this is a success
            AccessToken token = AccessToken.CurrentAccessToken;



            DataManager.Instance.SetUserAccessToken(token.TokenString);
            DataManager.Instance.SetUserState(Construct._ONE);
            LoggedMessage.text = "LOGIN";
            FB.API("/me?fields=id", HttpMethod.GET, DisplayUsersId);
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsersFirstName);
            FB.API("/me?fields=last_name", HttpMethod.GET, DisplayUsersLastName);
            FB.API("/me/picture?type=square&height=200&width=200", HttpMethod.GET, DisplayUsersPic);
            FacebookLoginBtn.SetActive(false);
            FacebookLogoutBtn.SetActive(true);

        }
        else
        {
            //we had some error
            LoggedMessage.text = "FAILED LOGIN";
            Debug.Log("Failed Login");
           
        }
    }

    private void DisplayUsersPic(IGraphResult results)
    {
        if (results.Texture != null)
        {

            if (DataManager.Instance.GetUserId() != Construct._USERID && DataManager.Instance.GetUserId() != Construct._USERNAME && DataManager.Instance.GetUserId() != Construct._USERACCESSTOKEN)
            {

                string userPicture = "https://graph.facebook.com/" + DataManager.Instance.GetUserId() + "/picture?width=200";
                DataManager.Instance.new2dpicture(DataManager.Instance.UserImagePic, userPicture);
                DataManager.Instance.SetUserPic(userPicture);
                DataManager.Instance.SetUserName(DataManager.Instance.GetUserFirstName() + " " + DataManager.Instance.GetUserLastName());
                DataManager.Instance.SetUserActivation(Construct._ONE);
                ServerStatusManager.Instance.SendNewDataType(Construct.ONLOGIN);
                WelcomeMessage.text = "Welcome, " + DataManager.Instance.GetUserFirstName() + " To " + SystemConfig.Instance.AppName;
            }
            else
            {
                Debug.Log("THE PICTURE IS NOT WHAT IT SHOULD BE");
            }
        }
        else
        {
            //we had a picture error
            Debug.Log("THE PICTURE IS NOT WHAT IT SHOULD BE WE HAVE SOME ERROR FROM FACEBOOK");

        }
    }
    private void DisplayUsersFirstName(IResult results)
    {
        if (results.Error == null)
        {
            //ever thing is ok 
         
           
            DataManager.Instance.SetUserFirstName(results.ResultDictionary["first_name"].ToString());

        }
        else
        {
            //everything is not ok;

            
        }
    }

    private void DisplayUsersLastName(IResult results)
    {
        if (results.Error == null)
        {
            //ever thing is ok 
            

            DataManager.Instance.SetUserLastName(results.ResultDictionary["last_name"].ToString());
        }
        else
        {
            //everything is not ok;

            
        }
    }

    private void DisplayUsersId(IResult results)
    {
        if (results.Error == null)
        {
            //ever thing is ok 
            
            DataManager.Instance.SetUserId(results.ResultDictionary["id"].ToString());

        }
        else
        {
            //everything is not ok;

           
        }
    }

   

}
