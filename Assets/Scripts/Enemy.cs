using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 10f;

    Transform _target;
    int _wavepointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _target = AIPath.Points[0];
    }

    // Update is called once per frame
    void Update()
    {
        var dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.2f) {
            GetNextPoint();
        }
    }

    void GetNextPoint() {
        if (_wavepointIndex >= AIPath.Points.Length - 1) {
            Destroy(gameObject);
            return;
        }

        _wavepointIndex++;
        _target = AIPath.Points[_wavepointIndex];
    }
}
