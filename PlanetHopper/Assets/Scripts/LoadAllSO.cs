using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAllSO : MonoBehaviour
{
    public LevelInformation[] levels;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
