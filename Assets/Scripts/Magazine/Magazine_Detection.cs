using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Magazine_Detection : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] GameObject _mag_Gameobject;
    [SerializeField] Sniper _sniper;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Magazine"))
        {
            _mag_Gameobject = collision.collider.gameObject;
            _playerData.m_MagazineLoaded = true;
            _audioSource.clip = _playerData.m_MagAudioClip;
                _audioSource.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Magazine"))
        {
            if (collision.collider.gameObject == _mag_Gameobject)
            {
                //Mag is now inserted
                _sniper.PlayHaptics(new HapticClipPlayer(_sniper.m_Magazine_Haptics), Controller.Left);
                _sniper.PlayHaptics(new HapticClipPlayer(_sniper.m_Magazine_Haptics), Controller.Right);
                _playerData.m_MagazineLoaded = true;
                _audioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Magazine"))
        {
            if (collision.collider.gameObject == _mag_Gameobject)
            {
                //Mag is now released
                _playerData.m_MagazineLoaded = false;
                _audioSource.clip = _playerData.m_ReloadAudioClip;
                _audioSource.Play();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            _mag_Gameobject = other.gameObject;
            _playerData.m_MagazineLoaded = true;
            _audioSource.clip = _playerData.m_MagAudioClip;
                _audioSource.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            if (other.gameObject == _mag_Gameobject)
            {
                //Mag is now inserted
                _playerData.m_MagazineLoaded = true;
                _audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            if (other.gameObject == _mag_Gameobject)
            {
                //Mag is now released
                _playerData.m_MagazineLoaded = false;
                _audioSource.clip = _playerData.m_ReloadAudioClip;
                _audioSource.Play();
            }
        }
    }
}
