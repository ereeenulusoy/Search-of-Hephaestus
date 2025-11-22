using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private Transform _boxCastOriginPoint;
    [SerializeField] private Vector3 _boxCastSize;
    [SerializeField] private Vector3 _boxCastOffset;
    public GameObject[] DEBUG_DETECTED;

    public bool debug;

    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        AlignTo(entrancePoint, targetSnapPoint);

        SnapTo(entrancePoint, targetSnapPoint);
    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        Quaternion targetRotation = targetSnapPoint.transform.rotation;
        targetRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation = targetRotation * Quaternion.Inverse(ownSnapPoint.transform.localRotation);
    }

    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        var offset = transform.position - ownSnapPoint.transform.position;
        var newPosition = targetSnapPoint.transform.position + offset;

        float correctionAmount = 0.015f;

        Vector3 pushBack = targetSnapPoint.transform.forward * correctionAmount;

        newPosition += pushBack;

        transform.position = newPosition;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugLogDetectedObjects();
        }
    }

    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);
    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        foreach (SnapPoint snapPoint in snapPoints)
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
    public void CloseOtherOneWithCap(SnapPoint selectedSnapPoint)
    {
        SnapPoint[] points = GetComponentsInChildren<SnapPoint>();
        
        foreach (SnapPoint point in points)
        {
            point.ToggleCaps(false);
        }

        List<SnapPoint> filteredPoints = points.Where(x => x != selectedSnapPoint).ToList();

        foreach (SnapPoint point in filteredPoints)
        {
            if (point.pointType == SnapPointType.Enter) continue;

            point.ToggleCaps(true);
        }
    }
    public void DebugLogDetectedObjects()
    {
        if (_boxCastOriginPoint == null || !debug) return;

        Vector3 center = _boxCastOriginPoint.TransformPoint(_boxCastOffset);
        Vector3 halfExtents = _boxCastSize * 0.5f;
        Quaternion rotation = _boxCastOriginPoint.rotation;

        int layerMask = ~0;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation, layerMask, QueryTriggerInteraction.Ignore);

        List<GameObject> found = new List<GameObject>();
        foreach (Collider hit in hits)
        {
            if (hit == null) continue;

            if (hit.GetComponentInParent<SnapPoint>() != null) continue;

            if (hit.transform.IsChildOf(transform) || hit.transform == transform)
            {
                continue;
            }

            if (hit.attachedRigidbody != null && hit.attachedRigidbody.transform.IsChildOf(transform)) continue;

            found.Add(hit.gameObject);
        }

        DEBUG_DETECTED = found.ToArray();
    }
    public bool DetectCollision()
    {
        if (_boxCastOriginPoint == null) return false;

        Vector3 center = _boxCastOriginPoint.TransformPoint(_boxCastOffset);
        Vector3 halfExtents = _boxCastSize * 0.5f;
        Quaternion rotation = _boxCastOriginPoint.rotation;

        int layerMask = ~0; 
        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation, layerMask, QueryTriggerInteraction.Ignore);

        List<GameObject> found = new List<GameObject>();
        foreach (Collider hit in hits)
        {
            if (hit == null) continue;

            if (hit.GetComponentInParent<SnapPoint>() != null) continue;

            if (hit.transform.IsChildOf(transform) || hit.transform == transform) continue;

            if (hit.attachedRigidbody != null && hit.attachedRigidbody.transform.IsChildOf(transform)) continue;

            found.Add(hit.gameObject);
        }

        if (found.Count > 0)
        {
            DEBUG_DETECTED = found.ToArray();
            return true;
        }

        DEBUG_DETECTED = null;
        return false;
    }
    public (Vector3 boxCastPosition,Vector3 boxCastEuler, Vector3 boxCastSize) GetBoxCastInfo()
    {
        if (_boxCastOriginPoint == null) return (Vector3.zero, Vector3.zero, Vector3.zero);

        return (_boxCastOriginPoint.position + _boxCastOffset, _boxCastOriginPoint.rotation.eulerAngles, _boxCastSize);
    }

    private void OnDrawGizmosSelected()
    {
        if (_boxCastOriginPoint == null) return;

        Gizmos.color = Color.red;

        Matrix4x4 prevMatrix = Gizmos.matrix;
        Gizmos.matrix = _boxCastOriginPoint.localToWorldMatrix;
        Gizmos.DrawWireCube(_boxCastOffset, _boxCastSize); 
        Gizmos.matrix = prevMatrix;
    }
}