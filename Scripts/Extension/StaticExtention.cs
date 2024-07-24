using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticExtention 
{
    public static Vector3 ConvetToTarget(this Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x , 0, targetPosition.z);
        return targetPosition;
    }
}
