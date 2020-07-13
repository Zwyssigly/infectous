using UnityEngine;

static class MathUtility
{
    public static bool IsWithinAngle(float local, float start, float end)
    {
        float d1 = Mathf.DeltaAngle(start, end);
        float d2 = Mathf.DeltaAngle(start, local);

        return d1 > 0 ? d2 > 0 && d2 <= d1 : d2 < 0 && d2 >= d1;
    }

    public static Vector3 PointOnCircle(float angle, float radius)
    {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;
    }

    public static float AngleBetween(Vector3 current, Vector3 target)
    {
        var wheelDiff = current - target;
        return Mathf.Atan2(wheelDiff.y, wheelDiff.x) / Mathf.PI * 180;
    }

    public static Vector2 AngleAsNormal(float angle)
    {
        return new Vector2(Mathf.Cos(angle / 180f * Mathf.PI), Mathf.Sin(angle / 180f * Mathf.PI));
    }
}
