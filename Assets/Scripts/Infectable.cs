using System.Linq;
using UnityEngine;

public class Infectable : MonoBehaviour
{
    private WheelVisual visual;

    public Species species = Species.Neutral;
    public float infectionDuration = 1f;

    private Infectable infecting;
    public int infectingIndex { get; private set; }
    private int infectingCount;
    private int infectingSpread;
    private float infectionTime;

    public AudioSource infectSound;
    public AudioClip explodeSound;

    void Start()
    {
        visual = GetComponent<WheelVisual>();
    }

    public void BeginInfection(Infectable infectable)
    {
        if (species == infectable.species)
        {
            return;
        }

        infecting = infectable;
        infectingIndex = visual.outerInstances.Select((item, index) => (item, index)).OrderBy(item => Vector3.Distance(item.item.transform.position, infectable.transform.position)).First().index;
        infectingCount = Mathf.CeilToInt((visual.outerInstances.Count - 1f)  / 2f);
        infectingSpread = 0;
        infectionTime = 0;
        InfectOuter(infectingIndex);

        SendMessage("OnBeginInfection", infectable, SendMessageOptions.DontRequireReceiver);
        if (infectSound != null) infectSound.Play();
    }

    private void InfectOuter(int index)
    {
        visual.outerInstances[((index % visual.outerInstances.Count) + visual.outerInstances.Count) % visual.outerInstances.Count]
            .GetComponent<SpriteRenderer>().color = SpeciesColor.For(infecting.species);
    }

    public void AbortInfection()
    {
        if (species != Species.Neutral && infecting != null)
        {
            infecting = null;
            ReverseInfection();
            SendMessage("OnAbortInfection", SendMessageOptions.DontRequireReceiver);
            if (infectSound != null) infectSound.Pause();
        }
    }

    private void ReverseInfection()
    {
        foreach (var o in visual.outerInstances)
            o.GetComponent<SpriteRenderer>().color = SpeciesColor.For(species);
    }

    // Update is called once per frame
    void Update()
    {
        if (infecting != null)
        {
            infectionTime += Time.deltaTime;

            var spread = (int)(infectionTime / infectionDuration * infectingCount);
            for (int i = infectingSpread+1; i <= spread; i++)
            {
                InfectOuter(infectingIndex + i);
                InfectOuter(infectingIndex - i);
            }
            infectingSpread = spread;

            if (spread >= infectingCount)
            {
                var infecting = this.infecting;
                this.infecting = null;

                var args = new EndInfectionArgs();
                SendMessage("OnEndInfection", args, SendMessageOptions.DontRequireReceiver);
                if (infectSound != null) infectSound.Pause();

                if (args.Abort)
                {
                    ReverseInfection();
                }
                else
                {
                    visual.innerInstance.GetComponent<SpriteRenderer>().color = SpeciesColor.For(infecting.species);
                    species = infecting.species;                   
                }
            }
        }
    }
}

public class EndInfectionArgs
{
    public bool Abort = false;
}

public enum Species
{
    Neutral = 0,
    Friend = 1,
    Enemy = 2,
}

public static class SpeciesColor
{
    private static Color32[] colors =
    {
        new Color32(216, 137, 49, 255),
        new Color32(13, 140, 30, 255),
        new Color32(207, 17, 17, 255),
    };
    public static Color For(Species index) => colors[(int)index];
}
