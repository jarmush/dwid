using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{

    public Player player;       //Public variable to store a reference to the player game object
    Vector3 offset = new Vector3(0,0,-3);

    // Use this for initialization
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 6);


        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 1, 15);
        }

    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {

            transform.position = player.transform.position + offset;

    }
}