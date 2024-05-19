using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Magazine"))
        {
            return;
        }
    }
}
