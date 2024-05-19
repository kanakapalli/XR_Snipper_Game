using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class EntrySceneManager : MonoBehaviour
{
    [Header("Button References"), Tooltip("Buttons that plays an important role in the start scene")]
    #region Button References
    [SerializeField] Button _scanRoom_Button;
    [SerializeField] Button _proceedRoom_Button;
    [SerializeField] Button _gameScene_Button;
    [SerializeField] Button _next_Button;
    [SerializeField] Button _prev_Button;
    #endregion

    [Header("Scriptable References"), Tooltip("Scriptable objects that helps to store data and keep tracks")]
    #region Scriptable References
    [SerializeField] SceneStateManager _sceneStateManager;
    [SerializeField] PlayerData _playerData;
    #endregion

    [Header("Panels References"), Tooltip("Panels that are used to view and navigate visually")]
    #region Panels
    [SerializeField] GameObject _Start_Panel;
    [SerializeField] GameObject _Contract_Panel;
    #endregion

    [SerializeField] int _scene_index;
    [SerializeField] TMP_Text m_Complexity_Level;

    private spwanAtKeyWall m_SpawnAtKeyWall;

    private void Awake()
    {
        m_SpawnAtKeyWall = GetComponent<spwanAtKeyWall>();
    }

    private void Start()
    {
        ButtonAction();
        SwitchPanel(_playerData.m_Visited);
        _scene_index = PlayerPrefs.GetInt("Scene", 0);
    }

    private void ButtonAction()
    {
        _scanRoom_Button.onClick.AddListener(() =>
        {
            //On Click Room Will Be Scanned.
            Debug.Log("Scan The Room");

        });

        _proceedRoom_Button.onClick.AddListener(() =>
        {
            //On Click Game Will Be Proceeded To Next Scene i.e. Game Scene
            Debug.Log("Proceed To The Main Scene");
            if (_scene_index == 1)
            {
                SwitchPanel(true);
                if (_playerData.m_NewPlayerStatus == NewPlayerStatus.GoodJob)
                {
                    _playerData.m_NewPlayerStatus = NewPlayerStatus.Important;
                    m_SpawnAtKeyWall.spwanContactor(m_SpawnAtKeyWall.m_Important);
                }
            }
            else
            {
                _sceneStateManager.NextScene(1);
            }
        });

        _gameScene_Button.onClick.AddListener(() =>
        {
            //_sceneStateManager.NextScene(2);
            SceneManager.LoadScene("MainScene");
            _sceneStateManager._sceneIndex = 2;
        });

        _next_Button.onClick.AddListener(() =>
        {
            if (_playerData.m_Level >= -1 && _playerData.m_Level < 5)
            {
                _playerData.m_Level++;
                m_Complexity_Level.text = (_playerData.m_Level).ToString();
            }
        });

        _prev_Button.onClick.AddListener(() =>
        {
            if (_playerData.m_Level <= 6 && _playerData.m_Level > 0)
            {
                _playerData.m_Level--;
                m_Complexity_Level.text = (_playerData.m_Level).ToString();
            }
        });
    }

    private void SwitchPanel(bool _config)
    {
        if (!_config)
        {
            _Start_Panel.SetActive(true);
            _Contract_Panel.SetActive(false);
        }
        else
        {
            _Start_Panel.SetActive(false);
            _Contract_Panel.SetActive(true);
        }
    }
}
