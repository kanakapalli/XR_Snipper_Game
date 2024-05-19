using Oculus.Interaction.PoseDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Scope_Zoom : MonoBehaviour
{
    [SerializeField] Transform _main_Object;

    [SerializeField] GameObject _main_Camera;
    [SerializeField] GameObject _scope_Camera;
    [SerializeField] GameObject _scope_Object;
    [SerializeField] GameObject _scope_Canvas;

    [SerializeField] Volume volume;
    [SerializeField] Vignette vignette;
    [SerializeField] LayerMask _zoomedLayerMask;
    [SerializeField] LayerMask _unzoomedLayerMask;
    [SerializeField] UniversalRendererData _URP_Asset_Data;

    [SerializeField] PlayerData m_Player_Data;

    private void Start()
    {
        _main_Camera = Camera.main.gameObject;
        _scope_Canvas.SetActive(false);
        /*if (volume.profile.TryGet<Vignette>(out vignette) == false)
        {
            Debug.LogError("Vignette not found on Volume");
        }*/
    }

    private void Update()
    {
        //Debug.Log("<color=yellow>" + CalculateDistance() + "</color>");
        if (CalculateDistance() < .14f)
        {
            RaycastHit _hit;
            if (Physics.Raycast(_main_Camera.transform.position,
                _main_Camera.transform.forward,
                out _hit,
                m_Player_Data.m_Sniper_AWM_Speed,
                m_Player_Data.m_Scope_Effect_Layer) &&
                _main_Camera.transform.parent.parent.rotation.y >= (_main_Object.rotation.y - 5) || _main_Camera.transform.parent.parent.rotation.y <= (_main_Object.rotation.y + 5))
            {
                if (_scope_Canvas.activeSelf)
                {
                    if (_hit.collider.CompareTag("Scope_BG"))
                    {
                        ScopeActive();
                    }
                    else
                    {
                        ScopeInactive();
                    }
                }
                else
                {
                    ScopeActive();
                }
            }
        }
        else
        {
            ScopeInactive();
        }

        // Get the vertical input from the right hand joystick
        float input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y;

        // Modify the field of view based on the input
        _scope_Camera.GetComponent<Camera>().fieldOfView += input;

        // Clamp the field of view between 2 and 60
        _scope_Camera.GetComponent<Camera>().fieldOfView = Mathf.Clamp(_scope_Camera.GetComponent<Camera>().fieldOfView, 2, 40);
    }

    private float CalculateDistance()
    {
        float distance = 0;
        distance = Vector3.Distance(_main_Camera.transform.position, _scope_Object.transform.position);
        return distance;
    }

    private void SetIntensity(float intensity)
    {
        // Set the intensity of the Vignette
        vignette.intensity.value = intensity;
    }

    private void ScopeActive()
    {
        //SetIntensity(.5f);
        _scope_Canvas.SetActive(true);
        //_URP_Asset_Data.opaqueLayerMask = _zoomedLayerMask;
        _main_Camera.GetComponent<Camera>().cullingMask = _zoomedLayerMask;
    }

    private void ScopeInactive()
    {
        //SetIntensity(0f);
        _scope_Canvas.SetActive(false);
        //_URP_Asset_Data.opaqueLayerMask = _unzoomedLayerMask;
        _main_Camera.GetComponent<Camera>().cullingMask = _unzoomedLayerMask;
    }
}
