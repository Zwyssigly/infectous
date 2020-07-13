using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SideEffect : MonoBehaviour
{
    protected Infectable thisInfectable;
    protected Infectable otherInfectable;

    public void OnBeginInfection(Infectable infectable)
    {
        thisInfectable = GetComponent<Infectable>();
        if (thisInfectable.species == Species.Enemy && infectable.species == Species.Friend && this.isActiveAndEnabled)
        {
            otherInfectable = infectable;
            StartEffect();
        }
    }

    void OnEndInfection(EndInfectionArgs args)
    {
        if (otherInfectable != null)
        {
            if (!EndEffect())
                args.Abort = true;

            otherInfectable = null;
        }
    }

    void OnAbortInfection()
    {
        if (otherInfectable != null)
        {
            AbortEffect();
            otherInfectable = null;
        }
        }

    protected abstract void StartEffect();

    protected abstract bool EndEffect();

    protected abstract void AbortEffect();
}
