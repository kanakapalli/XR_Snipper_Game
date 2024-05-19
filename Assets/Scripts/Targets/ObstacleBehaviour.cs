using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] internal List<GameObject> _randomStaticObstacle = new List<GameObject>();
    [SerializeField] internal List<DynamicMovingObstacle> _randomDynamicObstacle = new List<DynamicMovingObstacle>();
    [SerializeField] int _level = 0;
    [SerializeField] int _last_level = 6;
    [SerializeField] float _level_time = 4f;

    private void Start()
    {
        //Static Obstacle Random Spawning
        StaticObjectMovement();
        DynamicObjectMovement();
    }

    private void Update()
    {
        //Dynamic Obstacle Movement
        /*       DynamicObjectMovement();*/
    }

    private void DynamicObjectMovement()
    {
        foreach (var obstacle in _randomDynamicObstacle)
        {
            StartCoroutine(KeepMoving(obstacle, true));
        }
    }

    private void StaticObjectMovement()
    {
        /*foreach (var obstacle in _randomStaticObstacle)
        {
            StartCoroutine(RandomPlacement(obstacle, true));
        }*/
        StartCoroutine(RandomPlacement(true));
    }

    private IEnumerator KeepMoving(DynamicMovingObstacle obstacle, bool _config)
    {
        Transform p1 = obstacle._point_a;
        Transform p2 = obstacle._point_b;
        Transform obj = obstacle._object.transform;

        while (_config)
        {
            // Move towards p2
            UpdateRotation(p1, p2, obj);
            while (Vector3.Distance(obj.position, p2.position) > 0.01f)
            {
                obj.transform.position = Vector3.MoveTowards(obj.position, p2.position, 2f * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(4f);

            // Move towards p1
            UpdateRotation(p2, p1, obj);
            while (Vector3.Distance(obj.position, p1.position) > 0.01f)
            {
                obj.transform.position = Vector3.MoveTowards(obj.position, p1.position, 2f * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(4f);
        }
    }


    private IEnumerator RandomPlacement(/*GameObject obstacle, */bool _config)
    {
        while (_config && _level <= _last_level)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < _level; i++)
            {
                int _index = Random.Range(0, _randomStaticObstacle.Count - 1);
                list.Add(_randomStaticObstacle[_index]);
            }
            StaticObstacleOperation(list, true);

            yield return new WaitForSeconds(_level_time);

            StaticObstacleOperation(list, false);
            _level++;
        }
    }

    private void UpdateRotation(Transform p1, Transform p2, Transform obj)
    {
        Vector3 _direction = p2.position - p1.position;
        Quaternion toRotation = Quaternion.LookRotation(_direction);
        obj.rotation = Quaternion.Lerp(obj.rotation, toRotation, 2f * Time.deltaTime);
    }

    private void StaticObstacleOperation(List<GameObject> _obstacles, bool _config)
    {
        if (_config)
        {
            AllStaticObstacleOperation(!_config); // _config will be true bydefault
            foreach (GameObject t in _obstacles)
            {
                t.SetActive(_config);
            }
        }
        else
        {
            AllStaticObstacleOperation(_config); // _config will be false bydefault
            /*foreach (GameObject t in _obstacles)
            {
                t.SetActive(!_config);
            }*/
        }
    }

    private void AllStaticObstacleOperation(bool _config)
    {
        foreach (var obs in _randomStaticObstacle)
        {
            obs.SetActive(_config);
        }
    }
}

[System.Serializable]
public struct DynamicMovingObstacle
{
    public Transform _point_a;
    public Transform _point_b;
    public GameObject _object;
}
