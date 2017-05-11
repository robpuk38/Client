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
    public string AppName = "AppName";


    private void Awake()
     {
         instance = this;
   

         if (!FB.IsInitialized)
         {
             FB.Init();
         }

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
                Debug.Log("OK YOU HAVE ENOUGH CREDITS TO JOIN");
               if (FB.IsLoggedIn)
                {
                    Debug.Log("We are alreay login");
                    //LoginStatusMemory();

                    LoggedMessage.text = "LOGIN";
                    FacebookLoginBtn.SetActive(false);
                    FacebookLogoutBtn.SetActive(true);
                    DataManager.Instance.SetUserState(Construct._ONE);
                    WelcomeMessage.text = "Welcome, " + DataManager.Instance.GetUserFirstName() + " To " + AppName;
                    ServerStatusManager.Instance.SendNewDataType(Construct.ONAWAKE);
                    HasLogout = false;

                    return;
                }
                else
                {
                    LoginStatusMemory();
                    return;
                }
            }
            else
            {
                Debug.Log("SEND POP UP TELLING THEM THEY NEED MORE CREDITS ");
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
            ServerStatusManager.Instance.SendNewDataType(Construct.ONLOGOUT);
            FacebookLoginBtn.SetActive(true);
            FacebookLogoutBtn.SetActive(false);

            DataManager.Instance.SetUserId(Construct._USERID);
            DataManager.Instance.SetUserName(Construct._USERGUEST);
            DataManager.Instance.SetUserPic(Construct._USERPIC);

            DataManager.Instance.UserImagePic.GetComponent<Image>().sprite = User;
            ServerStatusManager.Instance.CheckIfLoadedImage = false;
            ServerStatusManager.Instance.CheckAndSendOnce = false;
            HasLogout = true;
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
        if (FB.IsLoggedIn)
        {
            Debug.Log("Successful Login");
            //this is a success
            AccessToken token = AccessToken.CurrentAccessToken;



            DataManager.Instance.SetUserAccessToken(token.TokenString);
            DataManager.Instance.SetUserState("1");
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

            Debug.Log("Failed Login");
           
        }
    }

    private void DisplayUsersPic(IGraphResult results)
    {
        if (results.Texture != null)
        {
           
           
           
            string userPicture = "https://graph.facebook.com/" + DataManager.Instance.GetUserId() + "/picture?width=200";
            DataManager.Instance.new2dpicture(DataManager.Instance.UserImagePic, userPicture);
            DataManager.Instance.SetUserPic(userPicture);
            DataManager.Instance.SetUserName(DataManager.Instance.GetUserFirstName()+" "+ DataManager.Instance.GetUserLastName());
            DataManager.Instance.SetUserActivation(Construct._ONE);
            ServerStatusManager.Instance.SendNewDataType(Construct.ONLOGIN);
            WelcomeMessage.text ="Welcome, "+ DataManager.Instance.GetUserFirstName()+" To "+ AppName;
        }
        else
        {
            //we had a picture error

         
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

    /*

    private void LoginStatusMemory()
    {
        if (DataManager.Instance.GetUserId() != null
        && DataManager.Instance.GetUserId() != ""
        && DataManager.Instance.GetUserId() != "USERID"
        && DataManager.Instance.GetUserAccessToken() != null
        && DataManager.Instance.GetUserAccessToken() != ""
        && DataManager.Instance.GetUserAccessToken() != "USERACCESSTOKEN"
       )
        {
            // if we have already login before lets never do it again. 
            Debug.Log("We have already login before");
            MysqlManager.Instance.GetUsersData(DataManager.Instance.GetUserId(), DataManager.Instance.GetUserAccessToken());



            return;
        }
        else
        {
           
        }
    }

    int loadingtime = 0;
    int deloadingtime = 0;
    private void Update()
    {
        if (isloaded == true)
        {
            loadingtime++;
        }
        if (isloaded == false)
        {
            deloadingtime++;
        }
        if (loadingtime > 200 && isloaded == true && hasloaded == false)
        {
            if (LoadingManager.Instance != null)
            {
                LoadingManager.Instance.FadeOut();
            }
            FacebookLoginCanvas.SetActive(false);
            FacebookLogoutCanvas.SetActive(true);
            VungleCanvas.SetActive(true);
            AdcolonyCanvas.SetActive(true);
            hasloaded = true;
            loadingtime = 0;
        }

        if (deloadingtime > 200 && isloaded == false && hasloaded == false && hasLogout == true)
        {
            if (LoadingManager.Instance != null)
            {
                LoadingManager.Instance.FadeOut();
            }
            FacebookLoginCanvas.SetActive(true);
            FacebookLogoutCanvas.SetActive(false);
            VungleCanvas.SetActive(false);
            AdcolonyCanvas.SetActive(false);
            deloadingtime = 0;
            hasLogout = false;
        }
    }*/

   // public void MemoryData()
   // {

        
          /*  UserId.text = DataManager.Instance.GetUserId();

            UserName.text = DataManager.Instance.GetUserName();
            UserCredits.text = DataManager.Instance.GetUserCredits();
            UserLevel.text = DataManager.Instance.GetUserLevel();

            new2dpicture(UserPics, DataManager.Instance.GetUserPic());

            DataManager.Instance.SetUserState("1");
            DataManager.Instance.SaveUsersData();*/
        

   // }
    /*
    public void NoUserFound()
    {
        if (DataManager.Instance.GetUserPic() != null && DataManager.Instance.GetUserPic() != "" && DataManager.Instance.GetUserPic() != "USERPIC")
        {


            MysqlManager.Instance.PostUsersData(DataManager.Instance.GetUserId(),
               DataManager.Instance.GetUserPic(),
               DataManager.Instance.GetUserAccessToken(),
               DataManager.Instance.GetUserName(),
               DataManager.Instance.GetUserFirstName(),
               DataManager.Instance.GetUserLastName(),
               DataManager.Instance.GetUserState());
            Debug.Log("No User Was Found Inserting Now");
            return;
        }
        else
        {
            Debug.Log("No User Was Found Else Error");
            return;
        }
    }

    IEnumerator loadUsersPic(GameObject go, string url)
    {
       

        if (url.ToString() != null && url.ToString() != "" && url.ToString() != "USERPIC")
        {
            Texture2D temp = new Texture2D(0, 0);
            WWW www = new WWW(url);
            yield return www;

            temp = www.texture;
            Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            Transform themb = go.transform;
            themb.GetComponent<Image>().sprite = sprite;
            isloaded = true;

        }

    }


    private void new2dpicture(GameObject go, string url)
    {
        getuserspic = loadUsersPic(go, url);
        StartCoroutine(getuserspic);
    }


   

    private void GetUsersData(AccessToken token)
    {
        

    }

   

    public void FacebookLogout()
    {

        isloaded = false;
        hasloaded = false;
        hasLogout = true;
        deloadingtime = 0;
        LoadingManager.Instance.Fadein(false);
        DataManager.Instance.SetUserState("0");
        DataManager.Instance.SaveUsersData();
    }


    */

}
