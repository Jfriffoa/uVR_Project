using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class RotateWorld : MonoBehaviour
{
    public float degrees = 45f;
    public float speed;

    // Rotates Around Y Axis
    public void Rotate(BaseInputEventData data)
    {
        Debug.Log("ROTATE", gameObject);
        transform.Rotate(0, degrees, 0, Space.Self);
    }
}
