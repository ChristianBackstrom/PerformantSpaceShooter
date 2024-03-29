using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class MiscMath
{
    public static float GetHeading(float2 start, float2 end)
    {
        float x = start.x - end.x;
        float y = start.y - end.y;
        return math.atan2(y, x) + math.PI;
    }

    public static float GetDistanceSqr(float2 start, float2 end)
    {
        float2 offset = start - end;
        return offset.x * offset.x + offset.y * offset.y;
    }
}
