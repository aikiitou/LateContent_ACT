using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectController : MonoBehaviour
{
    private const float DISPLAYTIME = 0.1f;

    private float displaytimer = 0.0f;
    // Update is called once per frame
    void Update()
    {
        displaytimer += Time.deltaTime;
        if(displaytimer > DISPLAYTIME)
        {
            displaytimer = 0.0f;
            gameObject.SetActive(false);
        }
    }
}
