using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    void Awake()
    {
        BackgroundController[] objs = FindObjectsOfType<BackgroundController>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        transform.position = Camera.main.transform.position;
    }
}
