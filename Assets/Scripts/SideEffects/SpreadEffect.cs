using UnityEngine;

public class SpreadEffect : SideEffect
{
    private WheelVisual visual;

    public int virus = 0;
    public GameObject virusPrefab;

    protected override void StartEffect()
    {
        visual = GetComponent<WheelVisual>();
    }

    protected override void AbortEffect()
    {
    }


    protected override bool EndEffect()
    {
        for (int i = 0; i < virus; i++)
        {
            var angle = i * 360f / virus;
            var normal = MathUtility.AngleAsNormal(angle);
            VirusController.Spread(virusPrefab, transform.position + (Vector3)(normal * visual.scale / 2f), angle);
        }
        return true;
    }
}
