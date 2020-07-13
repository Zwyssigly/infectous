using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : SideEffect
{   

    protected override void StartEffect()
    {
        otherInfectable.BeginInfection(thisInfectable);
    }

    protected override void AbortEffect()
    {
        otherInfectable.AbortInfection();
    }

    protected override bool EndEffect()
    {
        otherInfectable.AbortInfection();
        return true;
    }
}
