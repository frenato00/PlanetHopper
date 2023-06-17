using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingCompass : MonoBehaviour
{

    private GameObject finish;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        finish = GameObject.FindWithTag("Finish");
        if(finish == null){
            Debug.LogError("No finish object found");
            this.gameObject.SetActive(false);
        }

        player = GameObject.FindWithTag("Player");
        if(player == null){
            Debug.LogError("No player object found");
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(finish.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
