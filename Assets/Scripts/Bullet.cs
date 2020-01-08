using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 70f;
    
    Rigidbody _rb;
    Transform _target;

    public void Seek(Transform target) {
        _target = target;
    }

    void Start() {
        _rb = GetComponent<Rigidbody>();
        speed *= transform.lossyScale.x;
    }

    void LateUpdate() {
        if (_target == null) {
            Debug.Log("No target. Destroying " + gameObject + "...");
            Destroy();
            return;
        }

        //Move towards target... with transform?
        Vector3 dir = _target.position - transform.position;
        float dist = speed * Time.deltaTime;
        transform.Translate(dir.normalized * dist, Space.World);
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("Colliding with..." + collision.gameObject);

        if (collision.transform == _target) {
            collision.gameObject.SendMessage("Kill");
            Destroy();
        }
    }

    void Destroy()
    {
        var particles = GetComponentInChildren<ParticleSystem>();

        //If we have particles, wait until the particles are finished to destroy the game object
        if (particles != null)
        {
            var main = particles.main;
            main.loop = false;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particles.Stop();

            GetComponent<Renderer>().enabled = false;

            Destroy(gameObject, 1f);

        //If we don't have particles, just destroy the game object
        } else {
            Destroy(gameObject);
        }
    }
}
