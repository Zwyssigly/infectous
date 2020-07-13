using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DecayController : MonoBehaviour
{
    public float startAngle;
    public float endAngle;
    public bool inverse;

    
    public bool IsWithinDecay(float angle)
    {
        return IsWithinDecayLocal(angle - transform.rotation.eulerAngles.z);
    }

    public bool IsWithinDecayLocal(float localAngle)
    {

        return inverse ^ MathUtility.IsWithinAngle(localAngle, startAngle, endAngle);
    }

    void OnDrawGizmos()
    {
        var visual = GetComponent<WheelVisual>();
        var from = MathUtility.PointOnCircle(startAngle, visual.scale / 2f);
        var to = MathUtility.PointOnCircle(endAngle, visual.scale / 2f);  

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + from, transform.position + to);
    }
}
