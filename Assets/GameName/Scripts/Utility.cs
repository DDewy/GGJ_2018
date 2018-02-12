using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector2Int NormalizeVec2Int(Vector2Int inputVec)
    {
        Vector2Int outputVec = new Vector2Int();

        if (inputVec.x != 0)
        {
            if (inputVec.x > 0)
            {
                outputVec.x = 1;
            }
            else
            {
                outputVec.x = -1;
            }
        }

        if (inputVec.y != 0)
        {
            if (inputVec.y > 0)
            {
                outputVec.y = 1;
            }
            else
            {
                outputVec.y = -1;
            }
        }

        return outputVec;
    }
}
