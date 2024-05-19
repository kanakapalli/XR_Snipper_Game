using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotSnap : MonoBehaviour
{
    internal void ActivateUpdateMode(Transform _wall, Transform _target)
    {
        UpdateRotation(_wall, _target);
    }

    private void UpdateRotation(Transform _transform, Transform _target)
    {
        //_target.position = Camera.main.transform.position;
        Vector3 direction = _transform.position - _target.position;
        direction.y = 0;
        float angle = Mathf.Atan2(direction.x, direction.z);
        angle = angle * Mathf.Rad2Deg;
        angle = -angle;

        Quaternion _rotation = Quaternion.Euler(0, angle, 0);
        _target.rotation = _rotation;
    }
}
