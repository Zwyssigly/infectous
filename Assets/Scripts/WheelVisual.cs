using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WheelVisual : MonoBehaviour
{
    [HideInInspector]
    public GameObject innerInstance;

    [HideInInspector]
    public List<GameObject> outerInstances = new List<GameObject>();

    public float scale = 1f;

    public GameObject innerWheel;
    public GameObject outerWheel;

    void Start()
    {
        var color = SpeciesColor.For(GetComponent<Infectable>()?.species ?? Species.Neutral);
        var decays = GetComponents<DecayController>();

        innerInstance = Instantiate(innerWheel, transform);
        innerInstance.transform.localScale = innerInstance.transform.localScale * scale;
        innerInstance.GetComponent<SpriteRenderer>().color = color;

        var ss = outerWheel.transform.localScale.x;
        var outerLine = (scale + ss / 2) * Mathf.PI;

        var count = (int) (outerLine / ss / 1.5);
        for (var i = 0; i < count; i++)
        {
            var angle = i * 360 / count;
            if (decays.All(d => !d.IsWithinDecayLocal(angle)))
            {
                var outerWheelInstance = Instantiate(outerWheel, transform);
                outerWheelInstance.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * ((scale + ss) / 2f);
                outerWheelInstance.transform.localRotation = Quaternion.Euler(0, 0, angle);
                outerWheelInstance.GetComponent<SpriteRenderer>().color = color;
                outerInstances.Add(outerWheelInstance);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = SpeciesColor.For(GetComponent<Infectable>()?.species ?? Species.Neutral);
        Gizmos.DrawSphere(transform.position, scale / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
