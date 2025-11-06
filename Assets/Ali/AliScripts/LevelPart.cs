using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private LayerMask intersectionLayer;
    [SerializeField] private Collider[] intersectioncheckcolliders;
    [SerializeField] private Transform intersectionCheckParent;

    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();
        foreach (var collider in intersectioncheckcolliders)
        {
            Collider[] hitColliders= Physics.OverlapBox(collider.bounds.center,collider.bounds.extents,Quaternion.identity, intersectionLayer);

            foreach (var hit in hitColliders)
            {
                IntersectionCheck intersectionCheck= hit.gameObject.GetComponent<IntersectionCheck>();

                if (intersectionCheck != null && intersectionCheckParent != intersectionCheck.transform) 
                    return true;
            }
        }
        return false;
    }

    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint= GetEntrancePoint();
        SnapTo(entrancePoint, targetSnapPoint);
        AlignTo(entrancePoint, targetSnapPoint);
    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y-transform.rotation.eulerAngles.y;

        transform.rotation= targetSnapPoint.transform.rotation;
        transform.Rotate(0, 180, 0);
        transform.Rotate(0,-rotationOffset,0);
    }
    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        var offset = transform.position- ownSnapPoint.transform.position;
        var newPosition = targetSnapPoint.transform.position + offset;
        transform.position = newPosition;
    }
    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);
    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        foreach(SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.pointType == pointType)
            {
                filteredSnapPoints.Add(snapPoint);
            }
        }
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }
        return null;
    }
}
