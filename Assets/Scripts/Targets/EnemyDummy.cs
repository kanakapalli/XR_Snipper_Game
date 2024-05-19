using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    [SerializeField] Animator _animator;

    private void Start()
    {
        _animator.SetBool("Push", false);
        _animator.SetBool("Died", true);
    }

    internal void Hurted()
    {
        StartCoroutine(EnemyHurt());
    }

    private IEnumerator EnemyHurt()
    {
        Debug.Log("<color=red>" + "Hurted Through Script" + "</color>");
        _animator.Play("Push");
        yield return new WaitForSeconds(.8f);
        Destroy(gameObject);

        /*_animator.SetBool("Died", false);
        yield return new WaitForSeconds(.8f);
        _animator.SetBool("Died", true);*/

        /*Destroy(gameObject);*/
    }
}
