using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;

    private void Update(){
        if(Input.GetMouseButton(0)){
            shootInput?.Invoke();
        }
    }
}
