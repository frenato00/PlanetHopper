using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{

    public Image BossBorder;
    public Image BossHealthBar;

    public Color BossBorderColorShielded;
    public Color BossBorderColorUnshielded;

    [HideInInspector] 
    public BossTarget bossTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(bossTarget.IsShielded());
        if(bossTarget.IsShielded()){
            BossBorder.color = BossBorderColorShielded;
        }else{
            BossBorder.color = BossBorderColorUnshielded;
        }


        BossHealthBar.fillAmount = bossTarget.GetHealth() / bossTarget.maxHealth;
        
    }
}
