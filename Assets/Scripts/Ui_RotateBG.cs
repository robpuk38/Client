using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_RotateBG : MonoBehaviour {

    public float x = 0;
    public float y = 0;
    public float z = 0;

   
    void Update () {
        this.transform.Rotate(x, y, z);
	}
}
