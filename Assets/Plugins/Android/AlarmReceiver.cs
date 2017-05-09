using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlarmReceiver : MonoBehaviour {
	[Header("TextCanvas")]
	public Text myText;
	[Tooltip("Tiempo ausente")]
	public float timeInterno;
	[Tooltip("Tiempo Interno por segundo")]
	public int tiempo;
	[Header("Lista Mensajes")]
	[Tooltip("mensajes para enviar")]
	public string myTitle;
	public string myLabel;
	void Start () {
		timeInterno = 0;
	}
	void Update () {
		if (nativeObj ==null){
			nativeObj = new AndroidJavaObject("com.macaronics.notification.AlarmReceiver");
		}
		CerrarApplicacion ();
	}
	// Update is called once per frame
	public void EnviarNotificaciones () {
		nativeObj.CallStatic("startAlarm", new object[4]{"No Descuides Tu Arbol", ""+myTitle+"", ""+myLabel+"", tiempo});
		Application.Quit();
	}
	void CerrarApplicacion(){
		if(Input.touchCount == 0){
			timeInterno = timeInterno + Time.deltaTime;
			if(timeInterno >=240){ //sistema autocerradopor ausencia
				EnviarNotificaciones();
				Application.Quit();
			}
		}
		if(Input.touchCount >0){ //toques en pantalla
			timeInterno=0;
		}
		if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape)){ //keys mobil
			EnviarNotificaciones();
			Application.Quit();
		}
	}
  	AndroidJavaObject nativeObj =null;
}