using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpoint
{
    // Start is called before the first frame update
    public void SaveCheckpoint();

    public void ResetGameState();
}