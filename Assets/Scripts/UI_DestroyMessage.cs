using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DestroyMessage : MonoBehaviour {



    public void OnClose()
    {
        Destroy(gameObject);
    }

}
