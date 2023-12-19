using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MIN_LENGTH = 2.5f;
    private const float MAX_LENGTH = 6.5f;

    [SerializeField]
    private GameObject player_object;

    private Vector3 camera_pos = Vector3.zero;
    private Vector3 player_pos = Vector3.zero;
    private float current_length = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        camera_pos = transform.position;
        player_pos = player_object.transform.position;
        current_length = camera_pos.x + player_pos.x;

    }

    // Update is called once per frame
    void Update()
    {
        camera_pos = transform.position;
        player_pos = player_object.transform.position;
        current_length = camera_pos.x - player_pos.x;
        if (current_length < MIN_LENGTH)
        {
            transform.position = new Vector3(player_pos.x + MIN_LENGTH,camera_pos.y,camera_pos.z);
        }

        if(current_length > MAX_LENGTH)
        {
            transform.position = new Vector3(player_pos.x + MAX_LENGTH, camera_pos.y, camera_pos.z);
        }
    }
}
