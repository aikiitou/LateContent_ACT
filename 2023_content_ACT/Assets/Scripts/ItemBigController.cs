using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBigController : MonoBehaviour
{
    private const float ADDVALUE = 50.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerController>().AddStamina(ADDVALUE);
            Destroy(gameObject);
        }
    }
}
