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

    void FixedUpdate() {
        if (_target == null) {
            Debug.Log("No target. Destroying " + gameObject + "...");
            Destroy(gameObject);
            return;
        }

        //Move towards target
        Vector3 dir = _target.position - transform.position;
        float dist = speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + dir.normalized * dist);
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Colliding with..." + collision.gameObject);

        if (collision.transform == _target) {
            collision.gameObject.SendMessage("Kill");
            Destroy(gameObject);
        }
    }
}
