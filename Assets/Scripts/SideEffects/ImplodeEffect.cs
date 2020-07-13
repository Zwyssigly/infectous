using System;
using UnityEngine;

public class ImplodeEffect : SideEffect
{
    private WheelVisual visual;
    private SpriteRenderer[] renderers;

    private float implodeDuration = 1f;
    private float implodeTime = 0f;
    private bool implode;

    void Update()
    {
        if (implode)
        {
            implodeTime += Time.deltaTime;
            var f = implodeTime / implodeDuration;

            visual.outerInstances.ForEach(i => i.transform.localPosition -= (i.transform.localPosition.normalized * Time.deltaTime));
            Array.ForEach(renderers, r => r.color = new Color(r.color.r, r.color.g, r.color.b, 1f - f));

            if (f >= 1f)
            {
                implode = false;
                DestroyImmediate(gameObject);
            }
        }
    }

    protected override void StartEffect()
    {
        visual = GetComponent<WheelVisual>();
        visual.innerInstance.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    protected override void AbortEffect()
    {
        visual.innerInstance.GetComponent<SpriteRenderer>().color = SpeciesColor.For(thisInfectable.species);
    }


    protected override bool EndEffect()
    {

        otherInfectable.GetComponent<PlayerController>().Jump();
        Destroy(thisInfectable.GetComponent<WheelController>());

        renderers = GetComponentsInChildren<SpriteRenderer>();
        implode = true;

        AudioSource.PlayClipAtPoint(thisInfectable.explodeSound, transform.position);
        return false;
    }
}
