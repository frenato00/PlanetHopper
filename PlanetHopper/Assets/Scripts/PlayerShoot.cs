using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;

    private void Update(){
        if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetMouseButton(0)){
            shootInput?.Invoke();
        }
    }
}
