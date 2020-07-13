using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera2d;    

    public BoxVisual floor;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        camera2d = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        camera2d.orthographicSize = Math.Min(5f, floor.width / 2f) / camera2d.aspect;

        float cameraHalfWidth = camera2d.aspect * camera2d.orthographicSize;
        
        var x = Mathf.Max(player.position.x, floor.transform.position.x - floor.width / 2f + cameraHalfWidth);
        x = Mathf.Min(x, floor.transform.position.x + floor.width / 2f - cameraHalfWidth);

        var y = Math.Max(player.position.y, floor.transform.position.y - floor.height / 2f + camera2d.orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
