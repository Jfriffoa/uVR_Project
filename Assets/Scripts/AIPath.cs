using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPath : MonoBehaviour
{
    public static Transform[] Points {
        get;
        private set;
    }

    void Awake() {
        Points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            Points[i] = transform.GetChild(i);
        }
    }
}
