using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour {

    private static DataManager instance;
    public static DataManager Instance {get { return instance; } }

    public Text Id;
    public Text UserId;
    public Text UserName;
    public Text UserPic;
    public Text UserFirstName;
    public Text UserLastName;
    public Text UserAccessToken;
    public Text UserState;
    public Text UserAccess;
    public Text UserCredits;
    public Text UserLevel;
    public Text UserMana;
    public Text UserHealth;
    public Text UserExp;
    public Text UserGpsX;
    public Text UserGpsY;
    public Text UserGpsZ;
    public Text UserActivation;
    public Text UserIpAddress;
    public Text UserFirstTimeLogin;
    public Text UserDeviceId;
    public Text UserXPos;
    public Text UserYPos;
    public Text UserZPos;
    public Text UserXRot;
    public Text UserYRot;
    public Text UserZRot;
    public Text UserAdsMod;
    public GameObject UserImagePic;
    public InputField UserinputCredits;


    private void Awake()
    {
        instance = this;
        UserinputCredits.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        Debug.Log("Value Changed");
        instance.SetUserAdsMod(Construct._ONE);
    }

    private IEnumerator getuserspic;
    IEnumerator loadUsersPic(GameObject go, string url)
    {
        Debug.Log("CALLLED MORE THEN IT SHOULD BE FOR SOME REASON ");

        if (url.ToString() != null && url.ToString() != "" && url.ToString() != "USERPIC")
        {
            Texture2D temp = new Texture2D(0, 0);
            WWW www = new WWW(url);
            yield return www;

            
            
                temp = www.texture;
                Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
                Transform themb = go.transform;
                themb.GetComponent<Image>().sprite = sprite;

           

        }

    }


    public void new2dpicture(GameObject go, string url)
    {
        getuserspic = loadUsersPic(go, url);
        StartCoroutine(getuserspic);

    }

    public void SetUserZRot(string set)
    {
        UserZRot.text = set;
    }

    public string GetUserZRot()
    {
        return UserZRot.text;
    }

    public void SetUserYRot(string set)
    {
        UserYRot.text = set;
    }

    public string GetUserYRot()
    {
        return UserYRot.text;
    }

    public void SetUserXRot(string set)
    {
        UserXRot.text = set;
    }

    public string GetUserXRot()
    {
        return UserXRot.text;
    }

    public void SetUserZPos(string set)
    {
        UserZPos.text = set;
    }

    public string GetUserZPos()
    {
        return UserZPos.text;
    }


    public void SetUserYPos(string set)
    {
        UserYPos.text = set;
    }

    public string GetUserYPos()
    {
        return UserYPos.text;
    }

    public void SetUserXPos(string set)
    {
        UserXPos.text = set;
    }

    public string GetUserXPos()
    {
        return UserXPos.text;
    }


    public void SetUserDeviceId(string set)
    {
        UserDeviceId.text = set;
    }

    public string GetUserDeviceId()
    {
        return UserDeviceId.text;
    }

    public void SetUserIpAddress(string set)
    {
        UserIpAddress.text = set;
    }

    public string GetUserIpAddress()
    {
        return UserIpAddress.text;
    }



    public void SetUserFirstTimeLogin(string set)
    {
        UserFirstTimeLogin.text = set;
    }

    public string GetUserFirstTimeLogin()
    {
        return UserFirstTimeLogin.text;
    }



    public void SetUserActivation(string set)
    {

        PlayerPrefs.SetString("UserActivation", set);
        UserActivation.text = set;
    }

    public string GetUserActivation()
    {
        if (PlayerPrefs.GetString("UserActivation") != "")
        {
            UserActivation.text = PlayerPrefs.GetString("UserActivation");
        }
        return UserActivation.text;
    }


    public void SetId(string set)
    {
       Id.text = set;
    }

    public string GetId()
    {
       return Id.text;
    }
    public void SetUserId(string set)
    {
        PlayerPrefs.SetString("UserId", set);
        UserId.text = set;
    }

    public string GetUserId()
    {
        if (PlayerPrefs.GetString("UserId") != "")
        {
           // Debug.Log("WE ARE GETTING THE USERS ID FROM SAVED GAME PREFS");
            UserId.text = PlayerPrefs.GetString("UserId");
        }
        return UserId.text;
    }

    public void SetUserAdsMod(string set)
    {
        
        UserAdsMod.text = set;
    }

    public string GetUserAdsMod()
    {
      
        return UserAdsMod.text;
    }
    

    public void SetUserName(string set)
    {
        PlayerPrefs.SetString("UserName", set);
        UserName.text = set;
    }

    public string GetUserName()
    {
        if (PlayerPrefs.GetString("UserName") != "" && FacebookManager.Instance.HasLogout == false)
        {
            //Debug.Log("WE ARE GETTING THE USERS NAME FROM SAVED GAME PREFS");
            UserName.text = PlayerPrefs.GetString("UserName");
        }
        return UserName.text;
    }


    public void SetUserPic(string set)
    {
        PlayerPrefs.SetString("UserPic", set);
        UserPic.text = set;
    }

    public string GetUserPic()
    {
        if (PlayerPrefs.GetString("UserPic") != "")
        {
           // Debug.Log("WE ARE GETTING THE USERS PIC FROM SAVED GAME PREFS");
            UserPic.text = PlayerPrefs.GetString("UserPic");
        }
        return UserPic.text;
    }

    public void SetUserFirstName(string set)
    {
        PlayerPrefs.SetString("UserFirstName", set);
        UserFirstName.text = set;
    }

    public string GetUserFirstName()
    {
        if (PlayerPrefs.GetString("UserFirstName") != "")
        {
            //Debug.Log("WE ARE GETTING THE USERS FIRST NAME FROM SAVED GAME PREFS");
            UserFirstName.text = PlayerPrefs.GetString("UserFirstName");
        }
        return UserFirstName.text;
    }

    public void SetUserLastName(string set)
    {
        PlayerPrefs.SetString("UserLastName", set);
        UserLastName.text = set;
    }

    public string GetUserLastName()
    {
        if (PlayerPrefs.GetString("UserLastName") != "")
        {
            //Debug.Log("WE ARE GETTING THE USERS LAST NAME FROM SAVED GAME PREFS");
            UserLastName.text = PlayerPrefs.GetString("UserLastName");
        }
        return UserLastName.text;
    }

    public void SetUserAccessToken(string set)
    {
        PlayerPrefs.SetString("UserAccessToken", set);
        UserAccessToken.text = set;
    }

    public string GetUserAccessToken()
    {
        if (PlayerPrefs.GetString("UserAccessToken") != "")
        {
           // Debug.Log("WE ARE GETTING THE USERS ACCESS TOKEN FROM SAVED GAME PREFS");
            UserAccessToken.text = PlayerPrefs.GetString("UserAccessToken");
        }
        return UserAccessToken.text;
    }

    public void SetUserState(string set)
    {
        
        UserState.text = set;
    }

    public string GetUserState()
    {
        return UserState.text;
    }

    public void SetUserAccess(string set)
    {
       
        UserAccess.text = set;
    }

    public string GetUserAccess()
    {
        return UserAccess.text;
    }

    public void SetUserCredits(string set)
    {
        UserinputCredits.text = set;
        UserCredits.text = set;
        
    }

    public string GetUserCredits()
    {
        return UserCredits.text;
    }

    public void SetUserLevel(string set)
    {

        UserLevel.text = set;
       
    }

    public string GetUserLevel()
    {
        return UserLevel.text;
    }

    public void SetUserMana(string set)
    {

        UserMana.text = set;
       
    }

    public string GetUserMana()
    {
        return UserMana.text;
    }

    public void SetUserHealth(string set)
    {

        UserHealth.text = set;
       
    }

    public string GetUserHealth()
    {
        return UserHealth.text;
    }

    public void SetUserExp(string set)
    {

        UserExp.text = set;
    }

    public string GetUserExp()
    {
        return UserExp.text;
    }

   

    public void SetUserGpsX(string set)
    {
        UserGpsX.text = set;
    }

    public string GetUserGpsX()
    {
        return UserGpsX.text;
    }

    public void SetUserGpsY(string set)
    {
        UserGpsY.text = set;
    }

    public string GetUserGpsY()
    {
        return UserGpsY.text;
    }

    public void SetUserGpsZ(string set)
    {
        UserGpsZ.text = set;
    }

    public string GetUserGpsZ()
    {
        return UserGpsZ.text;
    }



}
