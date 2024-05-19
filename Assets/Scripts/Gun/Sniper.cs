using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Oculus.Haptics;
using Unity.VisualScripting;
using Photon.Pun;

public class Sniper : MonoBehaviour
{
    [SerializeField] PlayerData m_Player_Data;
    [SerializeField] SceneStateManager m_SceneStateManager;
    [SerializeField] Transform m_Shoot_Pos;
    [SerializeField] GameObject m_Shoot_Prefab;
    [SerializeField] ParticleSystem m_Shoot_Muzzle;
    [SerializeField] Animator m_Shoot_Trigger_Animator;
    [SerializeField] Animator m_Shoot_Shell_Release_Animator;
    [SerializeField] GameObject _stainMaterial;
    [SerializeField] SnapInteractable m_Snap_Interactable;
    [SerializeField] Vector3 m_Original_Position;
    [SerializeField] TMP_Text m_Ammunation;
    [SerializeField] TMP_Text m_Status;
    [SerializeField] Image m_GunLoad;
    [SerializeField] Animator m_GunCanvas;

    [SerializeField] float m_recoil_amount = .1f;
    [SerializeField] float m_recoil_recovery_amount = .5f;

    private AudioSource m_Aud;
    private MainSceneManager m_MainSceneManager;

    public HapticClip m_Reload_Haptics;
    public HapticClip m_Magazine_Haptics;
    public HapticClip m_Shell_Haptics;
    public HapticClip m_Shot_Haptics;
    public HapticClip m_Rifle_Haptics;
    public HapticClip m_Water_Haptics;

    public HapticClip clip1;
    public HapticClip clip2;

    HapticClipPlayer _HapticLeft1;
    HapticClipPlayer _HapticLeft2;
    HapticClipPlayer _HapticRight1;
    HapticClipPlayer _HapticRight2;

    private void Start()
    {
        m_MainSceneManager = FindObjectOfType<MainSceneManager>();

        if (!m_Player_Data.m_Visited)
            m_Player_Data.m_Score = 0;
        m_Player_Data.m_Sniper_AWM_Ammo = 0;
        m_Player_Data.m_MagazineLoaded = false;
        m_Player_Data.m_Sniper_Shooting = false;
        m_Player_Data.m_Sniper_AWM_Enabled = false;
        m_Aud = GetComponent<AudioSource>();

        // We create two haptic clip players for each hand.
        _HapticLeft1 = new HapticClipPlayer(clip1);
        _HapticLeft2 = new HapticClipPlayer(clip2);
        _HapticRight1 = new HapticClipPlayer(clip1);
        _HapticRight2 = new HapticClipPlayer(clip2);

        // We increase the priority for the second player on both hands.
        _HapticLeft2.priority = 1;
        _HapticRight2.priority = 1;
    }

    private void Update()
    {
        //LeftController(_playerLeft1, _playerLeft2);
        //RightController(_playerRight1, _playerRight2);

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!m_Player_Data.m_Sniper_Shooting &&
                m_Player_Data.m_Sniper_AWM_Ammo > 0 &&
                m_Player_Data.m_MagazineLoaded &&
                m_Player_Data.m_Sniper_AWM_Enabled && SceneManager.GetActiveScene().name == "TutorialScene")
            {
                if (m_Player_Data.m_Score < 5)
                {
                    StartCoroutine(Shoot(3));
                }
                else
                {
                    //m_Player_Data.m_Visited = true;
                    //m_SceneStateManager.PreviousScene(1);
                    PlayerPrefs.SetInt("Scene", 1);
                    if (PlayerPrefs.GetInt("Scene", 0) == 1)
                    {
                        m_SceneStateManager._sceneIndex = 0;
                        SceneManager.LoadScene("StartScene");
                    }
                }
            }
            else if (!m_Player_Data.m_Sniper_Shooting &&
                m_Player_Data.m_Sniper_AWM_Ammo > 0 &&
                m_Player_Data.m_MagazineLoaded &&
                m_Player_Data.m_Sniper_AWM_Enabled && SceneManager.GetActiveScene().name == "MainScene")
            {
                StartCoroutine(Shoot(3));
            }
        }
        //Press The Button To Drop MAG
        if (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ReleaseMAG());
        }
        //Press The Button To Load MAG
        if (OVRInput.GetDown(OVRInput.RawButton.B) || Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(Reload(6));
        }

        /*        transform.localPosition = Vector3.Lerp(
                                                        transform.localPosition,
                                                        m_Original_Position,
                                                        m_recoil_recovery_amount * Time.deltaTime);*/
    }

    IEnumerator Shoot(float _time)
    {
        m_Player_Data.m_Sniper_Shooting = !m_Player_Data.m_Sniper_Shooting;
        m_Player_Data.m_Sniper_AWM_Ammo--;

        //Update the gun canvas that will be showing the status of the gun
        UpdateGunCanvas(m_Player_Data.m_Sniper_AWM_Ammo,
            "Loaded...", Color.green,
            m_Player_Data.m_Sniper_AWM_Ammo > 0 ? true : false,
            false);

        if (m_Player_Data.m_Sniper_AWM_Ammo <= 0)
        {
            StartCoroutine(ReleaseMAG());
            m_Player_Data.m_MagazineLoaded = false;
        }

        RightController(new HapticClipPlayer(m_Shot_Haptics), _HapticRight2);

        m_Shoot_Muzzle.Play();
        m_Aud.clip = m_Player_Data.m_ShootAudioClip;
        m_Aud.Play();

        m_Shoot_Shell_Release_Animator.Play("SniperFire");
        RaycastHit _hit;
        if (Physics.Raycast(m_Shoot_Pos.position,
            m_Shoot_Pos.forward,
            out _hit,
            m_Player_Data.m_Sniper_AWM_Speed,
            m_Player_Data.m_Bullet_Effect_Layer))
        {
            //_playerRight1 = new HapticClipPlayer(m_Shot_Haptics);
            //_playerLeft1 = new HapticClipPlayer(m_Shot_Haptics);


            Debug.Log("<color=yellow>" + _hit.collider.name + "</color>");
            var _bullet = Instantiate(m_Shoot_Prefab, m_Shoot_Pos.position, m_Shoot_Pos.rotation);
            Rigidbody _b = _bullet.GetComponent<Rigidbody>();
            _b.AddForce(m_Shoot_Pos.forward * m_Player_Data.m_Sniper_AWM_Speed);
            CreateStain(_hit.point, _hit.normal);

            if (_hit.transform.CompareTag("TargetEnemy"))
            {
                Transform _parent = _hit.transform.parent.parent.parent.parent.parent.parent.parent;
                if (_parent.CompareTag("TargetEnemy"))
                {
                    if (SceneManager.GetActiveScene().name == "MainScene")
                    {
                        EnemyDeath(_parent);
                    }
                }
            }
            else if (_hit.transform.CompareTag("TrainingDummy"))
            {
                Transform _parent = _hit.transform.parent.parent.parent.parent;
                _parent.GetComponent<EnemyDummy>().Hurted();
                m_Player_Data.m_Score++;
            }
            else if (_hit.transform.CompareTag("TrainingTarget"))
            {
                Debug.Log("<color=red>" + "Hurted Target" + "</color>");
                _hit.transform.GetComponent<EnemyDummy>().Hurted();
                m_Player_Data.m_Score++;
            }

            Destroy(_bullet, 3f);
        }
        StartCoroutine(GunLoad(_time));
        yield return new WaitForSeconds(_time);

        m_Player_Data.m_Sniper_Shooting = !m_Player_Data.m_Sniper_Shooting;
    }

    IEnumerator Reload(float _time)
    {
        if (!m_Player_Data.m_Sniper_AWM_Enabled && m_Player_Data.m_MagazineLoaded)
        {
            PlayHaptics(new HapticClipPlayer(m_Reload_Haptics), Controller.Left);
            PlayHaptics(new HapticClipPlayer(m_Reload_Haptics), Controller.Right);

            Debug.Log("<color=green>" + "Reloading..." + "</color>");//Indicating that gun is now reloading

            UpdateGunCanvas(m_Player_Data.m_Sniper_AWM_Ammo,
            "Reloading...", Color.red,
            m_Player_Data.m_Sniper_AWM_Ammo > 0 ? true : false,
            true);//Updating gun UI

            m_Aud.clip = m_Player_Data.m_ReloadAudioClip;//Assigning reload sound clip
            m_Aud.Play();//Playing reload sound clip
            m_Player_Data.m_Realoading = !m_Player_Data.m_Realoading;//Toggling reloading boolean value
            m_Status.GetComponent<Animator>().Play("StatusUpdate");
            //m_Status.GetComponent<Animator>().SetBool("Reloading", true);

            yield return new WaitForSeconds(_time);

            //m_Status.GetComponent<Animator>().SetBool("Reloading", false);
            m_Player_Data.m_Sniper_AWM_Ammo = 6;//After reloading, ammo full
            m_Player_Data.m_Realoading = !m_Player_Data.m_Realoading;//Toggling reloading boolean value
            m_Player_Data.m_Sniper_AWM_Enabled = true;//Enabling sniper to fire
            UpdateGunCanvas(m_Player_Data.m_Sniper_AWM_Ammo,
            "Loaded...", Color.green,
            m_Player_Data.m_Sniper_AWM_Ammo > 0 ? true : false,
            false);//Gun canvas Status change
        }
    }

    IEnumerator ReleaseMAG()
    {
        if (m_Player_Data.m_Sniper_AWM_Enabled)
        {
            PlayHaptics(new HapticClipPlayer(m_Shell_Haptics), Controller.Left);
            PlayHaptics(new HapticClipPlayer(m_Shell_Haptics), Controller.Right);

            m_Player_Data.m_Sniper_AWM_Ammo = 0;//Ammo Turned To Zero When Mag Is Released.
            UpdateGunCanvas(m_Player_Data.m_Sniper_AWM_Ammo,
                "Empty!!!", Color.red,
                m_Player_Data.m_Sniper_AWM_Ammo > 0 ? true : false,
                false); //Updating The Gun Canvas.

            m_Aud.clip = m_Player_Data.m_ShellAudioClip; //Assigning The Audio Clip
            m_Aud.Play(); //Playing The Audio Clip
            m_Snap_Interactable.enabled = false; //Making The Snap Interactable False To Make Magazine Fall

            yield return new WaitForSeconds(2f);

            m_Snap_Interactable.enabled = true; //Again Enabling Snap Intearctable To Be Able To Hold New Magazine
            m_Player_Data.m_Sniper_AWM_Enabled = false; //Turning Boolean To False So That After Releasing Mag Button Press Should Not Be Detected.
            m_Player_Data.m_MagazineLoaded = false; //Turning the boolean value for magazine loaded to false.
        }
    }

    void CreateStain(Vector3 position, Vector3 normal)
    {
        // Instantiate a stain object at the collision point with the given normal
        GameObject stain = Instantiate(_stainMaterial, position, Quaternion.identity);
        // Orient the stain to match the surface normal
        stain.transform.up = normal;
        Destroy(stain, 3f);
    }

    void EnemyDeath(Transform _enemy)
    {
        //_enemy.GetComponent<Animator>().SetBool("Death", true);
        m_MainSceneManager.Check(_enemy.transform.parent.parent.gameObject);
        _enemy.GetComponent<Animator>().enabled = false;
        _enemy.GetComponent<EnemyFollow>().enabled = false;
        _enemy.GetChild(0).gameObject.SetActive(false);
        _enemy.GetChild(1).gameObject.SetActive(true);
        Destroy(_enemy.gameObject, 4f);
    }

    void UpdateGunCanvas(int _ammunation, string _status, Color _status_color, bool _loaded, bool _reloading)
    {
        m_GunCanvas.Play("Canvas");
        m_Ammunation.text = "Ammo : " + _ammunation.ToString();
        m_Status.text = _status;
        m_Status.color = _status_color;
        m_Ammunation.GetComponent<Animator>().Play("ShootAmmo");
        /*if (_reloading)
        {
            m_Status.GetComponent<Animator>().SetBool("Reloading", true);
        }
        else
        {
            m_Status.GetComponent<Animator>().SetBool("Reloading", false);
        }*/
    }

    private IEnumerator GunLoad(float _time)
    {
        m_GunLoad.fillAmount = 0;
        float rate = 1 / _time;
        float progress = 0;

        while (progress < 1)
        {
            m_GunLoad.fillAmount = progress;
            progress += rate * Time.deltaTime;
            yield return null;
        }

        m_GunLoad.fillAmount = 1; // ensure fillAmount is set to 1 at the end
    }




    // This helper function allows us to identify the controller we are currently playing back on.
    // We use this further down for logging purposes.
    String GetControllerName(OVRInput.Controller controller)
    {
        if (controller == OVRInput.Controller.LTouch)
        {
            return "left controller";
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            return "right controller";
        }

        return "unknown controller";
    }

    // This section provides a series of interactions that showcase the playback and modulation capabilities of the
    // Haptics SDK.
    void HandleControllerInput(OVRInput.Controller controller, HapticClipPlayer clipPlayer1, HapticClipPlayer clipPlayer2, Controller hand)
    {
        string controllerName = GetControllerName(controller);

        try
        {
            // Play first clip with default priority using the index trigger
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                clipPlayer1.Play(hand);
                Debug.Log("Should feel vibration from clipPlayer1 on " + controllerName + ".");
            }

            // Play second clip with higher priority using the grab button
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller))
            {
                clipPlayer2.Play(hand);
                Debug.Log("Should feel vibration from clipPlayer2 on " + controllerName + ".");
            }

            // Stop first clip when releasing the index trigger
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                clipPlayer1.Stop();
                Debug.Log("Vibration from clipPlayer1 on " + controllerName + " should stop.");
            }

            // Stop second clip when releasing the grab button
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller))
            {
                clipPlayer2.Stop();
                Debug.Log("Vibration from clipPlayer2 on " + controllerName + " should stop.");
            }

            // Loop first clip using the B/Y-button
            if (OVRInput.GetDown(OVRInput.Button.Two, controller))
            {
                clipPlayer1.isLooping = !clipPlayer1.isLooping;
                Debug.Log(String.Format("Looping should be {0} on " + controllerName + ".", clipPlayer1.isLooping));
            }

            // Modulate the amplitude and frequency of the first clip using the thumbstick
            // - Moving left/right modulates the frequency shift
            // - Moving up/down modulates the amplitude
            if (controller == OVRInput.Controller.LTouch)
            {
                clipPlayer1.amplitude = Mathf.Clamp(1.0f + OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y, 0.0f, 1.0f);
                clipPlayer1.frequencyShift = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            }
            else if (controller == OVRInput.Controller.RTouch)
            {
                clipPlayer1.amplitude = Mathf.Clamp(1.0f + OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y, 0.0f, 1.0f);
                clipPlayer1.frequencyShift = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
            }
        }

        // If any exceptions occur, we catch and log them here.
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    internal void PlayHaptics(HapticClipPlayer m_Player, Controller m_Hand)
    {
        m_Player.Play(m_Hand);
    }

    private void LeftController(HapticClipPlayer _haptic_one, HapticClipPlayer _haptic_two)
    {
        HandleControllerInput(OVRInput.Controller.LTouch, _haptic_one, _haptic_two, Controller.Left);
    }

    private void RightController(HapticClipPlayer _haptic_one, HapticClipPlayer _haptic_two)
    {
        HandleControllerInput(OVRInput.Controller.RTouch, _haptic_one, _haptic_two, Controller.Right);
    }

    protected virtual void OnDestroy()
    {
        _HapticLeft1?.Dispose();
        _HapticLeft2?.Dispose();
        _HapticRight1?.Dispose();
        _HapticRight2?.Dispose();
    }

    // Upon exiting the application (or when playmode is stopped) we release the haptic clip players and uninitialize (dispose) the SDK.
    protected virtual void OnApplicationQuit()
    {
        Haptics.Instance.Dispose();
    }
}
