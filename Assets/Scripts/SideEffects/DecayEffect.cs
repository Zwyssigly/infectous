using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecayEffect :  SideEffect
{
    public float delta;

    private float startAngle;
    private float endAngle;
    private bool inverse;

    private PlayerController player;
    private WheelVisual visual;
    private List<SpriteRenderer> decayOuters;

    void Update()
    {

    }

    protected override void StartEffect()
    {
        visual = GetComponent<WheelVisual>();
        player = otherInfectable.GetComponent<PlayerController>();
        var localAngle = player.wheelAngle - transform.rotation.eulerAngles.z;
        var opposite = Mathf.DeltaAngle(0, localAngle + 180);
        
        inverse = delta >= 180;
        startAngle = opposite - delta / 2f;
        endAngle = opposite + delta / 2f;        

        decayOuters = visual.outerInstances
            .Where(i => inverse ^ MathUtility.IsWithinAngle(i.transform.localRotation.eulerAngles.z, startAngle, endAngle))
            .Select(g => g.GetComponent<SpriteRenderer>())
            .ToList();

        decayOuters.ForEach(i => i.color = Color.yellow);
    }

    protected override bool EndEffect()
    {
        var decay = gameObject.AddComponent<DecayController>();
        decay.startAngle = startAngle;
        decay.endAngle = endAngle;
        decay.inverse = inverse;

        decayOuters.ForEach(i =>
        {
            Destroy(i.gameObject);
            visual.outerInstances.Remove(i.gameObject);
        });
        player.UpdateDecay();

        AudioSource.PlayClipAtPoint(GetComponent<Infectable>().explodeSound, transform.position);
        return true;
    }

    protected override void AbortEffect()
    {
        //decayOuters.ForEach(i => i.color = Color.black);
    }
}
