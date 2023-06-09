using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour, IDamageable
{
    public UnityEvent onClick;
    public FMODUnity.EventReference clickSFX;

    void Awake() {
        if (onClick == null)
            onClick = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickSFX, transform.position);
        onClick.Invoke();
    }
}
