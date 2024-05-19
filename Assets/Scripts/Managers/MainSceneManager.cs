using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MainSceneManager : MonoBehaviour
{
    [SerializeField] List<GameObject> m_Follows = new List<GameObject>();
    [SerializeField] List<Levels> m_Levels = new List<Levels>();
    [SerializeField] PlayerData m_PlayerData;
    [SerializeField] GameObject m_EnemyPrefab;

    private void Start()
    {
        //m_Follows = GameObject.FindObjectsOfType<EnemyFollow>().Select(enemyFollow => enemyFollow.gameObject).ToList();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach(Transform _t in m_Levels[m_PlayerData.m_Level].POINTS)
        {
            var _enemy = Instantiate(m_EnemyPrefab, _t.position, Quaternion.identity);
            m_Follows.Add(_enemy);
        }
    }

    internal bool Check(GameObject hit)
    {
        if(m_Follows.Count == 1)
        {
            SceneManager.LoadSceneAsync("StartScene");
            return true;
        }
        else
        {
            FollowObjects(hit);
            return false;
        }
    }

    private void FollowObjects(GameObject _object)
    {
        GameObject _obj = new GameObject();
        foreach(GameObject obj in m_Follows)
        {
            if(obj == _object)
            {
                _obj = obj;
            }
        }
        m_Follows.Remove(_obj);
    }

    [System.Serializable]
    public struct Levels
    {
        public string LVL;
        public List<Transform> POINTS;
    }
}
