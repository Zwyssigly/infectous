using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    private Infectable thisInfectable;

    public float lifetime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        thisInfectable = GetComponent<Infectable>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var wheelInfectable = col.gameObject.GetComponent<Infectable>();
        if (wheelInfectable != null && wheelInfectable.species == Species.Neutral)
        {
            wheelInfectable.BeginInfection(thisInfectable);
            wheelInfectable.AbortInfection();
        }

        if (wheelInfectable == null) gameObject.SetActive(false);
    }

    public static void Spread(GameObject prefab, Vector3 position, float angle)
    {
        var instance = Instantiate(prefab, position, Quaternion.Euler(0, 0, angle));
        instance.GetComponent<Rigidbody2D>().velocity = MathUtility.AngleAsNormal(angle) * 10;
    }
}
