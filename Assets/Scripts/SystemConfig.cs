using UnityEngine;

public class SystemConfig : MonoBehaviour {

    private static SystemConfig instance;
    public static SystemConfig Instance { get { return instance; } }
    public string AppName = "App name";
    public string ServerIpAddress = "127.0.0.1";
    public int ServerPortNumber = 3000;
    public string VungleAppId = "VungleAppId";
    public string AdcolonyAppId = "AdcolonyAppId";
    public string AdcolonyZoneId = "AdcolonyZoneId";
    public GameObject ServerStatusSystem;
    public GameObject LoadManagers;
    private void Awake()
    {
        instance = this;
        ServerStatusSystem.SetActive(false);
        LoadManagers.SetActive(false);
    }

    private void Start()
    {
        ServerStatusSystem.SetActive(true);
        LoadManagers.SetActive(true);
    }
}
