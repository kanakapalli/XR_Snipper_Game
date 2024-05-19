using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] Transform[] _transforms;
    [SerializeField] Transform _fire_range;
    [SerializeField] Transform _player;

    private int random_index = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            random_index = Random.Range(0, _transforms.Length-1);
            StartCoroutine(Action(_transforms[random_index], _player));
        }
    }

    private IEnumerator Action(Transform wall, Transform target)
    {
        UpdateRotation(wall, target);
        yield return new WaitForSeconds(1f);
        UpdateFireRange();
    }

    private void UpdateRotation(Transform _transform, Transform _target)
    {
        Vector3 direction = _transform.position - _target.position;
        direction.y = 0;
        float angle = Mathf.Atan2(direction.x, direction.z);
        angle = angle * Mathf.Rad2Deg;
        angle = -angle;

        Quaternion _rotation = Quaternion.Euler(0, angle, 0);
        _target.rotation = _rotation;
    }

    private void UpdateFireRange()
    {
        _fire_range.position = _player.position;
        _fire_range.rotation = _player.rotation;
        UpdateRotation(_transforms[random_index], _fire_range.transform);
        UpdateWallRotation();
        _fire_range.transform.position += _fire_range.forward * 5;
    }

    private void UpdateWallRotation()
    {
        foreach(Transform _transform in _transforms)
        {
            _transform.rotation = _fire_range.rotation;
        }
    }
}
