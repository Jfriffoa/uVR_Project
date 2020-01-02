using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParent : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        transform.parent.SendMessage("OnTriggerEnter", other);
    }

    void OnTriggerExit(Collider other) {
        transform.parent.SendMessage("OnTriggerExit", other);
    }
}
