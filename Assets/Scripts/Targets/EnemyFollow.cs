using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] List<Transform> _follow_Targets;
    [SerializeField] Animator _followAnimator;
    [SerializeField] float speed = 2f; // Speed at which the enemy follows the target
    [SerializeField] float hold = 2f;
    [SerializeField] float radius = 50f;
    [SerializeField] float number_of_rays = 360f;
    [SerializeField] bool moving_target = true;
    [SerializeField] LayerMask target_layer;
    private int currentTargetIndex = 0; // Index of the current target in the list

    void Start()
    {
        _followAnimator = GetComponent<Animator>();
        _followAnimator.SetBool("Walking", false);

        StartCoroutine(Follow());

    }

    /*private void LateUpdate()
    {
        RayCast();
    }*/

    private void RayCast()
    {
        for (int i = 0; i < number_of_rays; i++)
        {
            float horizontalAngle = (i % (number_of_rays / 2)) * 360f / (number_of_rays / 2);
            float verticalAngle = (i / (number_of_rays / 2)) * 180f - 90f;

            Vector3 direction = new Vector3(
                radius * Mathf.Cos(verticalAngle) * Mathf.Cos(horizontalAngle),
                radius * Mathf.Sin(verticalAngle),
                radius * Mathf.Cos(verticalAngle) * Mathf.Sin(horizontalAngle)
            );

            Ray ray = new Ray(transform.position, direction.normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, radius, target_layer))
            {
                //Debug.DrawLine(ray.origin, hit.point, Color.red);
                // Do something with hit.point or hit.collider
                if ( hit.collider.gameObject.CompareTag("TargetEnemy"))
                {
                    Debug.DrawRay(ray.origin, ray.direction * radius, Color.cyan);
                    if (moving_target)
                    {
                        Normal(hit);
                        Debug.DrawRay(ray.origin, ray.direction * radius, Color.blue);
                    }
                    else
                    {
                        FightBack();
                        Debug.DrawRay(ray.origin, ray.direction * radius, Color.yellow);
                    }
                }
            }
            else
            {
                //Debug.DrawRay(ray.origin, ray.direction * radius, Color.green);
            }
        }
    }

    internal void Attentive()
    {
        moving_target = false;

        Debug.Log("Attentive");
    }

    internal bool CheckEnemy()
    {
        return moving_target;
    }

    private void Normal(RaycastHit hit)
    {
        if (hit.collider.gameObject.GetComponent<EnemyFollow>().CheckEnemy())
        {
            Debug.Log("Normal");
        }
        else
        {
            Attentive();
        }
    }

    private void FightBack()
    {
        Debug.Log("Fighting Back");
    }

    IEnumerator Follow()
    {
        while (moving_target)
        {
            // Get the current target
            Transform target = _follow_Targets[currentTargetIndex];

            // Move towards the target
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                _followAnimator.SetBool("Walking", true);

                // Rotate towards the target
                Vector3 direction = target.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                yield return null;
            }

            // Move to the next target
            currentTargetIndex = (currentTargetIndex + 1) % _follow_Targets.Count;
            _followAnimator.SetBool("Walking", false);

            //RayCast();
            yield return new WaitForSeconds(hold);
        }
    }
}
