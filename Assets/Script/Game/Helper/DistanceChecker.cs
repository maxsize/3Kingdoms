using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Helper class to detect distance in given range between destination and current position
/// </summary>
public class DistanceChecker
{
    public float Threashold = 0.1f;

    private Vector3 _destination;
    private Transform _transform;
    private float _lastDistance;

    public DistanceChecker(Vector3 destination, Transform transform)
    {
        _destination = destination;
        _transform = transform;
        _lastDistance = Vector3.Distance(_transform.position, _destination);
    }

    public bool Check()
    {
        var dist = Vector3.Distance(_transform.position, _destination);
        if (dist > _lastDistance)
            return true;    // Already moved beyond the target
        if (dist <= Threashold)
            return true;
        _lastDistance = dist;
        return false;
    }

    public void Dispose()
    {
        _transform = null;
    }
}
