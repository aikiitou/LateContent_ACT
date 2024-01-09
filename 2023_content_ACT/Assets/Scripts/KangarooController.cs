using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class KangarooController : MonoBehaviour
{
    float move_angle = 90.0f;

    //bool attackFlag = false;
    //float attackTimer = 0.0f;
    //Quaternion rot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    attackFlag = true;
        //}

        //if (attackFlag == true)
        //{
        //    attackTimer += Time.deltaTime;
        //    if (attackTimer < 0.5f)
        //    {
        //        rot = Quaternion.Euler(0.0f, 0.0f, -45f);

        //        transform.rotation = rot;
        //    }
        //    else
        //    {
        //        rot = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //        transform.rotation = rot;
        //        attackTimer = 0.0f;
        //        attackFlag = false;
        //    }
        //}
    }
}
