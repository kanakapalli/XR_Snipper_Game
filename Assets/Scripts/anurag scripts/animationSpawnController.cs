using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimationSpwanController : MonoBehaviour
{
    [Tooltip("Time in seconds after which the GameObject will be destroyed")]
    public float delay = 55.0f; // Time in seconds after which the GameObject will be destroyed


    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = FindFirstObjectByType<OVRCameraRig>().centerEyeAnchor;
        Destroy(gameObject, delay);
    }
    void Update()
    {
        Debug.Log("update is getting called");
        Vector3 newPosition = new Vector3(cameraTransform.position.x, 0f, cameraTransform.position.z);

        transform.LookAt(newPosition);
        Debug.DrawLine(transform.position, newPosition, UnityEngine.Color.red);
    }
}
