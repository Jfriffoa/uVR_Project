using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float maxDist = 0.2f;

    public float speed = 10f;
    public GameObject deathExplosion;

    Transform _target;
    int _wavepointIndex = 0;
    
    // Variables are scale depending
    void AdjustVars()
    {
        maxDist *= transform.lossyScale.x;
        speed *= transform.lossyScale.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        _target = AIPath.Points[0];
        transform.LookAt(_target);
        AdjustVars();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= maxDist) {
            GetNextPoint();
        }
    }

    void GetNextPoint() {
        if (_wavepointIndex >= AIPath.Points.Length - 1) {
            //Do Damage
            GameManager.Instance.Damage();

            Destroy(gameObject);
            return;
        }

        _wavepointIndex++;
        _target = AIPath.Points[_wavepointIndex];
        transform.LookAt(_target);
    }

    void Kill() {
        //Instantiate Particles
        var particles = Instantiate(deathExplosion, transform.position, transform.rotation, transform.parent);

        //Destroy  E V E R Y T H I N G
        Destroy(particles, 2f);
        Destroy(gameObject);
    }
}
