using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_Data_Storage", menuName = "Player_Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] internal string m_Player_Name;
    [SerializeField] internal int m_Sniper_AWM_Speed;
    [SerializeField] internal int m_Sniper_AWM_Ammo;
    [SerializeField] internal bool m_Sniper_AWM_Enabled;
    [SerializeField] internal bool m_Sniper_Shooting;
    [SerializeField] internal AudioClip m_ShootAudioClip;
    [SerializeField] internal AudioClip m_ShellAudioClip;
    [SerializeField] internal AudioClip m_ReloadAudioClip;
    [SerializeField] internal AudioClip m_MagAudioClip;
    [SerializeField] internal bool m_Realoading;
    [SerializeField] internal bool m_MagazineLoaded;
    [SerializeField] internal LayerMask m_Bullet_Effect_Layer;
    [SerializeField] internal LayerMask m_Scope_Effect_Layer;
    [SerializeField] internal int m_Score;
    [SerializeField] internal bool m_Visited;
    [SerializeField] internal SceneStateManager m_SceneStateManager;
    [SerializeField] internal int m_Level;

    [SerializeField] internal NewPlayerStatus m_NewPlayerStatus;

    internal event Action Reload;
    internal event Action Detach;

    #region Action Event
    internal void ReloadAmmo()
    {
        Reload?.Invoke();
    }
    internal void DetachAmmo()
    {
        Detach?.Invoke();
    }
    #endregion

    #region Action Methods
    internal void ReloadMethod()
    {

    }
    internal void DetachMethod()
    {

    }
    #endregion
}

public enum NewPlayerStatus
{
    Welcome,
    GoodJob,
    Important
}