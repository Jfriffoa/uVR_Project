using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Behaviour")]
    public string enemyTag = "Enemy";

    public float radius;
    public float turnSpeed = 10f;

    public GameObject sphereArea;
    public Collider playArea;

    Collider _parentCollider;
    Rigidbody _parentRb;

    Transform _target;
    Vector3 initialEuler;

    [Header("Shoot Behaviour")]
    public GameObject bulletPrefab;
    public Transform bulletContainer;
    public Transform firePoint;
    public float fireRate = 1f;

    float fireCountdown;
    bool _canFire = true;

    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        initialEuler = transform.rotation.eulerAngles;

        // Update the visual feedback to the radius of the turret
        sphereArea.transform.localScale = Vector3.one * 2 * radius;
        sphereArea.SetActive(false);

        // Get our variables to do the "snap" math
        _parentCollider = GetComponentInParent<Collider>();
        _parentRb = GetComponentInParent<Rigidbody>();

        // Parent of the prefab is the default container
        if (bulletContainer == null)
            bulletContainer = transform.parent.parent;

        // Seek for a "PlayArea" object if is not manually assigned
        if (playArea == null)
        {
            var go = GameObject.FindGameObjectWithTag("PlayArea");
            playArea = go.GetComponent<Collider>();
        }
    }

    void Update() {
        if (_target == null)
            return;

        //Look At me baby
        Quaternion lookDir = Quaternion.LookRotation(_target.position - transform.position);
        Quaternion newRot = Quaternion.Euler(initialEuler.x, initialEuler.y, lookDir.eulerAngles.y);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * turnSpeed);

        //Shoot me sweetie
        if (fireCountdown <= 0f && _canFire) {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius * transform.lossyScale.x);
    }

    void UpdateTarget() {
        _target = GetTarget();
    }

    Transform GetTarget() {
        //Check the colliders in the sphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius * transform.lossyScale.x);

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

    void Shoot() {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, bulletContainer);
        bullet.GetComponent<Bullet>().Seek(_target);
    }

    public void OnManipulationStarted() {
        _canFire = false;
        sphereArea.SetActive(true);
    }

    public void OnManipulationEnded() {
        _canFire = true;
        sphereArea.SetActive(false);
        CheckPlayArea();
    }

    //Check if we are inside the play area. If not, snap us
    void CheckPlayArea()
    {
        // How much should we move the object to snap it
        Vector3 delta = Vector3.zero;

        // Bounds of the play area
        var minArea = playArea.bounds.min;
        var maxArea = playArea.bounds.max;

        // Bounds of us
        var minTurret = _parentCollider.bounds.min;
        var maxTurret = _parentCollider.bounds.max;

        // MUST: MinArea <= MinTurret in each axis
        for (int i = 0; i < 3; i++)
        {
            if (minArea[i] > minTurret[i])
            {
                delta[i] += minArea[i] - minTurret[i];
            }
        }

        // MUST: MaxTurret <= MaxArea in each axis
        for (int i = 0; i < 3; i++)
        {
            if (maxTurret[i] > maxArea[i])
            {
                delta[i] += maxArea[i] - maxTurret[i];
            }
        }

        // Apply the new pos in the Rigidbody parent
        //transform.parent.Translate(delta, Space.World);
        _parentRb.MovePosition(_parentRb.position + delta);
    }
}
