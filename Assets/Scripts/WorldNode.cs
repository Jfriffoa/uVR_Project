using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;

public class WorldNode : MonoBehaviour {

    public Color hoverColor;

    Renderer _render;
    Color _idleColor;

    int _onTrigger = 0;

    void Start() {
        _render = GetComponent<Renderer>();
        _idleColor = _render.material.color;
    }

    internal void OnTriggerEnter(Collider other) {
        _onTrigger++;

        if (_onTrigger > 0) {
            _render.material.color = hoverColor;
        }
    }

    internal void OnTriggerExit(Collider other) {
        _onTrigger--;

        if (_onTrigger < 1) {
            _render.material.color = _idleColor;
        }
    }
}
