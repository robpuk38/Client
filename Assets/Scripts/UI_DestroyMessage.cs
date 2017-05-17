using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DestroyMessage : MonoBehaviour {

    private void Start()
    {
        OnClose();
    }

    public void OnClose()
    {
        Destroy(gameObject,10);
    }

   

}
