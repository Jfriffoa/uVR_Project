using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public string enemyTag = "Enemy";

    public float radius;
    public float turnSpeed = 10f;

    Transform _target;
    Vector3 initialEuler;

    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        initialEuler = transform.rotation.eulerAngles;
    }

    void Update() {
        if (_target == null)
            return;

        //Look At me baby
        Quaternion lookDir = Quaternion.LookRotation(_target.position - transform.position);
        Quaternion newRot = Quaternion.Euler(initialEuler.x, initialEuler.y, lookDir.eulerAngles.y);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void UpdateTarget() {
        _target = GetTarget();
    }

    Transform GetTarget() {
        //Check the colliders in the sphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        Transform target = null;
        float nearestDistance = 1000000;

        foreach (var collider in colliders) {
            if (!collider.CompareTag(enemyTag))
                continue;

            float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

            if (distanceToEnemy < nearestDistance) {
                nearestDistance = distanceToEnemy;
                target = collider.transform;
            }
        }

        return target;
    }
}
