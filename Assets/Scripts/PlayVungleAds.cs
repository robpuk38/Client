using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayVungleAds : MonoBehaviour
{


   public Text DebugText;
    public GameObject VungleBtn;
    public GameObject Disable;

    //public string AppId = "58d3030b64b3bf8742000250";
  
    bool CompleteAd = false;

    Dictionary<string, object> options;


   

    private void Start()
    {
        Init(SystemConfig.Instance.VungleAppId);
    }
   

    private void Init(string AppId)
    {
        Vungle.init(AppId, null, null);
        VungleBtn.GetComponent<Button>().enabled = false;
        Disable.SetActive(true);
        RequestAd();
    }

    private void RequestAd()
    {
        DebugText.text = "Request Vungle Ad";
        InitializeEventHandlers();
        options = new Dictionary<string, object>();
        options["incentivized"] = true;
    }

    private void InitializeEventHandlers()
    {
        Vungle.onAdStartedEvent += () => {
            DebugText.text = "On Ad Started";

            if (CompleteAd == false)
            {
                CompleteAd = true;
                int newcredits = 0;
                int.TryParse(DataManager.Instance.GetUserCredits(), out newcredits);
                int pushcredits = newcredits + 100;
                DataManager.Instance.SetUserCredits(pushcredits.ToString());
                ServerStatusManager.Instance.SendNewDataType(Construct.ONADS);

            }

        };
        


        Vungle.onAdFinishedEvent += (args) =>
        {
            DebugText.text = "On Ad Finished: "+ args;
            CompleteAd = false;

        };

        Vungle.adPlayableEvent += (adPlayable) => {
            DebugText.text = "This ad is playable: " + adPlayable.ToString();

            if (adPlayable)
            {
                DebugText.text = "An ad is ready to show!";
                VungleBtn.GetComponent<Button>().enabled = true;
                Disable.SetActive(false);
            }
            else
            {
                VungleBtn.GetComponent<Button>().enabled = false;
                Disable.SetActive(true);
                DebugText.text = "No ad is available at this moment.";
            }
        };
        

        Vungle.onLogEvent += (log) => {
            DebugText.text = "This log: "+log.ToString();
        };
    }

    public void PlayAd()
    {
        DebugText.text = "THE BUTTON WAS CLICKED PLAY AD";
        
        Vungle.playAdWithOptions(options);
        RequestAd();

    }

}
