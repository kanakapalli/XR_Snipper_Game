using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ControllerRayManipulate : MonoBehaviour
{
    [SerializeField] LayerMask _layer;
    [SerializeField] GameObject _cursor;
    [SerializeField] GameObject _target;

    private void Start()
    {
        _cursor = Instantiate(_cursor, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
    }

    private void Update()
    {
        Vector3 rightHandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion rightHandRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 rightHandForward = rightHandRotation * Vector3.forward;

        CastRay(rightHandPosition, rightHandForward, OVRInput.GetDown(OVRInput.RawButton.B));
    }

    private void CastRay(Vector3 origin, Vector3 direction, bool _config)
    {
        RaycastHit _hit;
        if (Physics.Raycast(origin, direction, out _hit, 200f, _layer))
        {
            if (_config)
            {
                var stain = Instantiate(_target, _hit.point, Quaternion.identity);
                stain.transform.up = _hit.normal;
            }
            else
            {
                _cursor.transform.position = _hit.point;
                _cursor.transform.up = _hit.normal;
            }
        }
    }
}
