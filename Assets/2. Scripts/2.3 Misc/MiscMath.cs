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
        return math.atan2(x, y) + math.PI;
    }
}
