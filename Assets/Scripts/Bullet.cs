using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;

    Rigidbody _rb;

    void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * speed);
    }

    void Update() {
        //_rb.MovePosition(_rb.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Colliding with..." + collision.gameObject);
    }
}
