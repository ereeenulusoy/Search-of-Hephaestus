using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1 reference
public enum SnapPointType { Enter, Exit }

// 0 references
public class SnapPoint : MonoBehaviour
{
    public SnapPointType pointType;

    // 0 references
    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + pointType.ToString();
    }
}