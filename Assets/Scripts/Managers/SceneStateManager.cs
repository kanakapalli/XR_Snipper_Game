using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneState", menuName = "StateManagers/Scene")]
public class SceneStateManager : ScriptableObject
{
    [SerializeField] internal List<Scene> _scenes;
    [SerializeField] internal SceneDetail _sceneDetail;
    [SerializeField] internal int _sceneIndex;
    [SerializeField] private List<SceneDetail> _sceneDetails = new List<SceneDetail>();

    private void OnEnable()
    {
        _sceneDetail = new SceneDetail { _index = 0, _name = Scene.StartScene };
        _sceneIndex = _sceneDetail._index;
        LoadScenes();
    }

    private void ChangeScene(string _scene)
    {
        SceneManager.LoadSceneAsync(_scene);
    }

    internal void NextScene(int _sceneProgress)
    {
        ChangeScene(GetSceneName(_sceneDetail._index, true, _sceneProgress));
    }

    internal void PreviousScene(int _sceneProgress)
    {
        ChangeScene(GetSceneName(_sceneDetail._index, false, _sceneProgress));
    }

    private string GetSceneName(int _index, bool _np, int _sceneProgress) //_np true for next and _np false for previous
    {
        foreach (var detail in _sceneDetails)
        {
            if (_scenes[_index] == detail._name)
            {
                if (_np)
                {
                    if (_sceneDetail._index < _scenes.Count)
                    {
                        _index+= _sceneProgress;
                        //_index++;
                        DebugColorConsole((_index).ToString(), "yellow");
                        _sceneDetail = new SceneDetail { _index = _index, _name = _scenes[_index] };
                        _sceneIndex = _sceneDetail._index;

                        break;
                    }
                }
                else
                {
                    if (_sceneDetail._index > -1)
                    {
                        _index-= _sceneProgress;
                        //_index--;
                        DebugColorConsole((--_index).ToString(), "red");
                        _sceneDetail = new SceneDetail { _index = _index, _name = _scenes[_index] };
                        _sceneIndex = _sceneDetail._index;

                        break;
                    }
                }
            }
        }
        return _sceneDetail._name.ToString();
    }

    private void LoadScenes()
    {
        _sceneDetails.Clear();
        int i = 0;
        foreach (var scene in _scenes)
        {
            _sceneDetails.Add(new SceneDetail { _index = i, _name = scene });
            i++;
        }
    }

    private void DebugColorConsole(string _message, string _color)
    {
        Debug.Log(string.Concat("<color=", _color, ">", _message, "</color>"));
    }

    internal struct SceneDetail
    {
        internal int _index;
        internal Scene _name;
    }

    internal enum Scene
    {
        StartScene,
        TutorialScene,
        MainScene,
        TestScene
    }
}
