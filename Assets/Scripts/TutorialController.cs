using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    private GameObject canvas;

    public int count = 1;
    public bool staysOn = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (count-- > 0)
            canvas.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!staysOn)
            canvas.SetActive(false);
    }
}
