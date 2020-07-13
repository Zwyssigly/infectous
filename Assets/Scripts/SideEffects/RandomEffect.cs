using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEffect : MonoBehaviour
{
    private Infectable thisInfectable;

    void Start()
    {
        thisInfectable = GetComponent<Infectable>();
    }

    void OnBeginInfection(Infectable infectable)
    {
        if (infectable.species == Species.Friend && thisInfectable.species == Species.Enemy)
        {
            var effects = GetComponents<SideEffect>();
            if (effects.Count(f => f.enabled) > 0)
                return;

            effects = effects.Where(f => !f.enabled).ToArray();
            if (effects.Length > 0)
            {
                var effect = effects[Random.Range(0, effects.Length)];
                effect.enabled = true;
                effect.OnBeginInfection(infectable);
            }
        }
    }
}
