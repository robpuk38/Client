using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpContationManager : MonoBehaviour {
    private static PopUpContationManager instance;
    public static PopUpContationManager Instance { get { return instance; } }
    public Text Title;
    public Text Body;
    public GameObject RequirementsInfo;
    public GameObject InAppBuyCredits;
    public GameObject InAppBuyGMStatus;
    public GameObject InAppBuyADMINStatus;

    private void Awake()
    {
        instance = this;
    }

    public void Toggle()
    {
       
        Debug.Log("WHAT IS THIS "+ EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name);
        InappButtonJoinDeActive();
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name == "Close")
        {
            PopUpContainor("","");
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name == "FacebookBtn")
        {
            PopUpContainor("FAcebook Join", "Before You Can Join Our Service It Is Required To Have At Least 200 Or More Credits  Or A Paid Account." +
                " We Have Provided Each User With The Option To Either Join Via Paid With Google In-App Payments Or With Viewing And Downloading At Least One App, " +
                "From Our Advertisement Partners, Our Service Is Not Free And We Are Not A Non- Profit Organization, " +
                "All money Earned Is For Profit And Services To Provide Our Members Better Services.");
            InappButtonJoinActive();
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name == "FacebookBtnInfo")
        {
            PopUpContainor("Facebook Login", " Joining With Facebook, It Is Required To Have At Least 200 Credits To Join Our Service," +
                " It Is To Be Noted, To Inform You Of The Data We Collect Upon Login, Our Services Stores And Saves Your Facebook UserId, " +
                "Along With Your Facebook Username, And Also But Not Least We Store Your Facebook Profile Picture. " +
                "The Data We Collect Insures You And Your Game Status To Be Identified From Your Personal Unquie ID Facebook Provides. " +
                "At Any Giving Moment You Can Deactivate Your Account From Our Service, Once Deactivated It Can No Longer Be Used, " +
                "However If You Return And Reactivate, The Server Will Continue Uninterrupted As If You Never Left. " +
                "As A Reminder The Data Collected Is Always Stored For You To Reactive.");
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name == "SettingsBtnInfo")
        {
            PopUpContainor("Settings Option", "Something About Settings!");
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name == "VungleBtnInfo")
        {
            PopUpContainor("Vungle Ads", "Our In-App Video Ads And Innovative Templets Are Designed To Blend Into The User Experience We’ve Carefully Created For Our Audiences. " +
                "That’s Why Vungle Ads Simply Perform Better, Which In Turn, Helps Us Grow Our Mobile Business. " +
                "We’re Proud Of Our Track Record Of Promoting Other Publishers To Help Generate More Revenue. " +
                "In Fact, Some Of Our Customers Have Seen A 10x Increase In Monetization! " +
                "We Know Some Of Our Clients Are Unable To Pay For Service, So Using Advertisements Like Vungle We can Include All Members To Join Us. " +
                "Any Client As Long As They Have The Required Credits Are Able To Join. Using Vungle Allows Us To Provide Better Service So All Can Join!");
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>().name == "AdcolonyBtnInfo")
        {
            PopUpContainor("AdColony Ads", "Our In-App Video Ads And Innovative Templets Are Designed To Blend Into The User Experience We’ve Carefully Created For Our Audiences. " +
                "That’s Why AdColony Ads Simply Perform Better, Which In Turn, Helps Us Grow Our Mobile Business. " +
                "We’re Proud Of Our Track Record Of Promoting Other Publishers To Help Generate More Revenue. " +
                "In Fact, Some Of Our Customers Have Seen A 10x Increase In Monetization! " +
                "We Know Some Of Our Clients Are Unable To Pay For Service, So Using Advertisements Like AdColony We can Include All Members To Join Us. " +
                "Any Client As Long As They Have The Required Credits Are Able To Join. Using AdColony Allows Us To Provide Better Service So All Can Join!");
        }

        if (RequirementsInfo.activeSelf == true)
        {
            RequirementsInfo.SetActive(false);
        }
        else
        {
            RequirementsInfo.SetActive(true);
        }
    }

    private void InappButtonJoinActive()
    {
        InAppBuyCredits.SetActive(true);
        InAppBuyGMStatus.SetActive(true);
        InAppBuyADMINStatus.SetActive(true);
    }
    private void InappButtonJoinDeActive()
    {
        InAppBuyCredits.SetActive(false);
        InAppBuyGMStatus.SetActive(false);
        InAppBuyADMINStatus.SetActive(false);
    }
    private void PopUpContainor(string _Title, string _Body)
    {
        Title.text = _Title;
        Body.text = _Body;
    }
	
	
}
