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
    }

    void FixedUpdate() {
        if (_target == null) {
            Debug.Log("No target. Destroying " + gameObject + "...");
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float dist = speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + dir.normalized * dist);
    }

    //TODO: POLISH CON PARTICULAS
    void OnCollisionEnter(Collision collision) {
        Debug.Log("Colliding with..." + collision.gameObject);

        if (collision.transform == _target) {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
