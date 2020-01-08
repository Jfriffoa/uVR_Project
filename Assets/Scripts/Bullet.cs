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
            Destroy(gameObject);
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
            Destroy(gameObject);
        }
    }
}
